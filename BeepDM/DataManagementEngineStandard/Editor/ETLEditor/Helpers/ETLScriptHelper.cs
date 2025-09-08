using System;
using System.Collections.Generic;
using System.Threading;
using TheTechIdea.Beep.Editor.ETLEditor;
using TheTechIdea.Beep.Utilities;
using System.Linq;

namespace TheTechIdea.Beep.Editor.ETLEditor
{
    internal class ETLScriptHelper
    {
        private readonly IDMEEditor _dme;
        private readonly ETLEditor _etl;

        public ETLScriptHelper(IDMEEditor dme, ETLEditor etl)
        {
            _dme = dme ?? throw new ArgumentNullException(nameof(dme));
            _etl = etl; // may be null in some tests
        }

        public List<ETLScriptDet> GetCreateEntityScript(IDataSource ds, List<string> entities, IProgress<PassedArgs> progress, CancellationToken token, bool copydata = false)
        {
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            var rt = new List<ETLScriptDet>();
            try
            {
                var ls = new List<EntityStructure>();
                foreach (var item in entities)
                {
                    EntityStructure t1 = ds.GetEntityStructure(item, true);
                    if (t1 != null) ls.Add(t1);
                }
                rt.AddRange(GetCreateEntityScript(ds, ls, progress, token, copydata));
            }
            catch (Exception ex)
            {
                _dme?.AddLogMessage("Fail", $"Error in getting entities from Database ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return rt;
        }

        public List<ETLScriptDet> GetCreateEntityScript(IDataSource Dest, List<EntityStructure> entities, IProgress<PassedArgs> progress, CancellationToken token, bool copydata = false)
        {
            _dme.ErrorObject.Flag = Errors.Ok;
            int i = 0;
            List<ETLScriptDet> retval = new List<ETLScriptDet>();
            try
            {
                foreach (EntityStructure item in entities)
                {
                    ETLScriptDet copyscript = GenerateScript(item, Dest.DatasourceName, DDLScriptType.CreateEntity);
                    copyscript.ID = i;
                    copyscript.CopyData = copydata;
                    copyscript.IsCreated = false;
                    copyscript.IsModified = false;
                    copyscript.IsDataCopied = false;
                    copyscript.Failed = false;
                    copyscript.errormessage = string.Empty;
                    copyscript.Active = true;
                    copyscript.Mapping = new EntityDataMap_DTL();
                    copyscript.Tracking = new List<SyncErrorsandTracking>();

                    retval.Add(copyscript);
                    i++;
                }

                _dme.AddLogMessage("Success", $"Generated Script", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                _dme.AddLogMessage("Fail", $"Error in Generating Script: {ex.Message}", DateTime.Now, 0, null, Errors.Failed);
            }
            return retval;
        }

        public ETLScriptDet GenerateScript(EntityStructure item, string destSource, DDLScriptType scriptType)
        {
            var upscript = new ETLScriptDet();
            upscript.sourcedatasourcename = item.SourceDataSourceID;
            upscript.sourceentityname = item.EntityName;
            upscript.sourceDatasourceEntityName = string.IsNullOrEmpty(item.DatasourceEntityName) ? item.EntityName : item.DatasourceEntityName;
            upscript.destinationDatasourceEntityName = string.IsNullOrEmpty(item.DatasourceEntityName) ? item.EntityName : item.DatasourceEntityName;
            upscript.destinationentityname = item.EntityName;
            upscript.destinationdatasourcename = destSource;
            upscript.SourceEntity = item;
            upscript.scriptType = scriptType;
            return upscript;
        }

        public List<ETLScriptDet> GetCopyDataEntityScript(IDataSource Dest, List<EntityStructure> entities, IProgress<PassedArgs> progress, CancellationToken token)
        {
            _dme.ErrorObject.Flag = Errors.Ok;
            int i = 0;
            List<ETLScriptDet> retval = new List<ETLScriptDet>();
            try
            {
                foreach (EntityStructure sc in entities)
                {
                    ETLScriptDet copyscript = GenerateScript(sc, Dest.DatasourceName, DDLScriptType.CopyData);
                    copyscript.ID = i;
                    i++;
                    retval.Add(copyscript);
                }
                _dme.AddLogMessage("Success", $"Generated Script", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                _dme.AddLogMessage("Fail", $"Error in Generating Script: {ex.Message}", DateTime.Now, 0, null, Errors.Failed);
            }
            return retval;
        }
    }
}
