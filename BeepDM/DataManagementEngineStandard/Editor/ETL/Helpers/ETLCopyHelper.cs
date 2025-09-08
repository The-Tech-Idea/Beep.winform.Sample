using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Editor.ETL;
using System.ComponentModel;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Workflow.Mapping;

namespace TheTechIdea.Beep.Editor.ETL
{
    /// <summary>
    /// Helper class for ETL copy operations
    /// Handles data copying between sources with proper error handling and progress reporting
    /// </summary>
    internal class ETLCopyHelper
    {
        private readonly IDMEEditor _dme;
        private readonly ETLEditor _etl;

        public ETLCopyHelper(IDMEEditor dme, ETLEditor etl)
        {
            _dme = dme ?? throw new ArgumentNullException(nameof(dme));
            _etl = etl;
        }

        /// <summary>
        /// Copies entity data from source to destination with progress reporting
        /// </summary>
        public IErrorsInfo CopyEntityData(IDataSource sourceds, IDataSource destds, string srcentity, string destentity, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            try
            {
                _etl.errorcount = 0;
                EntityStructure item = sourceds.GetEntityStructure(srcentity, true);
                if (item != null)
                {
                    if (destds.Category == DatasourceCategory.RDBMS)
                    {
                        IRDBSource rDB = (IRDBSource)destds;
                        rDB.DisableFKConstraints(item);
                    }

                    if (!destds.CheckEntityExist(destentity))
                    {
                        _dme.AddLogMessage("Copy Data", $"Error Could not Copy Entity Data {srcentity} on {sourceds.DatasourceName} to {srcentity} on {destds.DatasourceName} ", DateTime.Now, 0, null, Errors.Failed);
                        return _dme.ErrorObject;
                    }

                    var src = Task.Run(() => { return sourceds.GetEntity(item.EntityName, null); });
                    src.Wait();
                    var srcTb = src.Result;
                    IList<object> srcList = null;

                    if (srcTb != null)
                    {
                        // Convert source data to list based on type
                        if (srcTb.GetType().FullName.Contains("DataTable"))
                        {
                            srcList = _dme.Utilfunction.GetListByDataTable((DataTable)srcTb, DMTypeBuilder.MyType, item);
                        }
                        else if (srcTb is IBindingListView blv)
                        {
                            srcList = new List<object>();
                            foreach (var o in blv) srcList.Add(o);
                        }
                        else if (srcTb is IList<object> listObj)
                        {
                            srcList = listObj;
                        }
                        else if (srcTb is System.Collections.IEnumerable ie)
                        {
                            srcList = ie.Cast<object>().ToList();
                        }

                        if (srcList != null)
                        {
                            int localCount = srcList.Count();
                            _etl.ScriptCount += localCount;
                            int i = 0;
                            foreach (var r in srcList)
                            {
                                i++;
                                _dme.ErrorObject = _etl.InsertEntity(destds, item, destentity, null, r, progress, token);
                                token.ThrowIfCancellationRequested();
                                // report progress
                                if (progress != null)
                                {
                                    PassedArgs ps = new PassedArgs { ParameterInt1 = _etl.CurrentScriptRecord, ParameterInt2 = _etl.ScriptCount, Messege = _dme.ErrorObject.Message };
                                    progress.Report(ps);
                                }
                            }
                        }
                    }

                    if (destds.Category == DatasourceCategory.RDBMS)
                    {
                        IRDBSource rDB = (IRDBSource)destds;
                        rDB.EnableFKConstraints(item);
                    }
                }
                else
                {
                    _dme.AddLogMessage("Copy Data", $"Error Could not Find Entity {srcentity} on {sourceds.DatasourceName}", DateTime.Now, 0, null, Errors.Failed);
                    _etl.errorcount = (int)_etl.StopErrorCount;
                }
            }
            catch (Exception ex)
            {
                _dme.AddLogMessage("Fail", $"Error copying Data {srcentity} on {sourceds.DatasourceName} to {srcentity} on {destds.DatasourceName} ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return _dme.ErrorObject;
        }

        /// <summary>
        /// Runs copy entity script with mapping support
        /// </summary>
        public IErrorsInfo RunCopyEntityScript(ETLScriptDet sc, IDataSource sourceds, IDataSource destds, string srcentity, string destentity, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            try
            {
                _etl.errorcount = 0;
                EntityStructure srcentitystructure = sourceds.GetEntityStructure(srcentity, true);
                EntityStructure destEntitystructure = destds.GetEntityStructure(destentity, true);
                if (srcentitystructure != null && destEntitystructure != null)
                {
                    if (destds.Category == DatasourceCategory.RDBMS)
                    {
                        IRDBSource rDB = (IRDBSource)destds;
                        rDB.DisableFKConstraints(destEntitystructure);
                    }

                    string querystring = srcentitystructure.EntityName;
                    List<AppFilter> filters = null;
                    List<EntityField> SelectedFields = null;
                    List<EntityField> SourceFields = null;

                    if (map_DTL != null)
                    {
                        SelectedFields = map_DTL.SelectedDestFields;
                        SourceFields = map_DTL.EntityFields;
                        querystring = srcentitystructure.EntityName;
                    }
                    else
                    {
                        SelectedFields = srcentitystructure.Fields;
                        SourceFields = srcentitystructure.Fields;
                    }

                    _dme.AddLogMessage("Info", $"Getting Data for {srcentity}", DateTime.Now, 0, null, Errors.Ok);
                    var src = Task.Run(() => { return sourceds.GetEntity(querystring, filters); });
                    src.GetAwaiter().GetResult();
                    var srcTb = src.Result;

                    IList<object> srcList = null;
                    if (srcTb != null)
                    {
                        if (srcTb.GetType().FullName.Contains("DataTable"))
                        {
                            srcList = _dme.Utilfunction.GetListByDataTable((DataTable)srcTb, DMTypeBuilder.MyType, srcentitystructure);
                        }
                        else if (srcTb is IBindingListView blv)
                        {
                            srcList = new List<object>();
                            foreach (var item in blv) srcList.Add(item);
                        }
                        else if (srcTb is IList<object> listObj)
                        {
                            srcList = listObj;
                        }
                        else if (srcTb is System.Collections.IEnumerable ie)
                        {
                            srcList = ie.Cast<object>().ToList();
                        }

                        if (srcList != null)
                        {
                            int localCount = srcList.Count();
                            _etl.ScriptCount += localCount;
                            int i = 0;
                            foreach (var r in srcList)
                            {
                                i++;
                                _dme.ErrorObject = _etl.InsertEntity(destds, destEntitystructure, destentity, map_DTL, r, progress, token);
                                token.ThrowIfCancellationRequested();
                            }
                        }
                    }

                    if (destds.Category == DatasourceCategory.RDBMS)
                    {
                        IRDBSource rDB = (IRDBSource)destds;
                        rDB.EnableFKConstraints(srcentitystructure);
                    }
                }
                else
                {
                    _dme.AddLogMessage("Copy Data", $"Error Could not Find Entity {srcentity} on {sourceds.DatasourceName}", DateTime.Now, 0, null, Errors.Failed);
                    _etl.errorcount = (int)_etl.StopErrorCount;
                }
            }
            catch (Exception ex)
            {
                _dme.AddLogMessage("Fail", $"Error copying Data {srcentity} on {sourceds.DatasourceName} to {srcentity} on {destds.DatasourceName} ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return _dme.ErrorObject;
        }
    }
}