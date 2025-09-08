using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Editor.ETL;
using TheTechIdea.Beep.Workflow.Mapping;

namespace TheTechIdea.Beep.Editor.ETL
{
    /// <summary>
    /// Helper class for ETL script execution
    /// Handles running scripts, progress reporting, and error tracking
    /// </summary>
    internal class ETLExecutionHelper
    {
        private readonly IDMEEditor _dme;
        private readonly ETLEditor _etl;

        public ETLExecutionHelper(IDMEEditor dme, ETLEditor etl)
        {
            _dme = dme ?? throw new ArgumentNullException(nameof(dme));
            _etl = etl;
        }

        /// <summary>
        /// Runs child scripts asynchronously
        /// </summary>
        public async Task<IErrorsInfo> RunChildScriptAsync(ETLScriptDet ParentScript, IDataSource srcds, IDataSource destds, IProgress<PassedArgs> progress, CancellationToken token)
        {
            if (ParentScript.CopyDataScripts.Count > 0)
            {
                for (int i = 0; i < ParentScript.CopyDataScripts.Count; i++)
                {
                    ETLScriptDet sc = ParentScript.CopyDataScripts[i];
                    destds = _dme.GetDataSource(sc.destinationdatasourcename);
                    srcds = _dme.GetDataSource(sc.sourcedatasourcename);
                    if (destds != null && srcds != null)
                    {
                        _dme.OpenDataSource(sc.destinationdatasourcename);
                        _dme.OpenDataSource(sc.sourcedatasourcename);
                        if (destds.ConnectionStatus == ConnectionState.Open)
                        {
                            if (sc.scriptType == DDLScriptType.CopyData)
                            {
                                SendMessege(progress, token, null, sc, $"Started Coping Data for Entity  {sc.destinationentityname}  in {sc.destinationdatasourcename}");
                                _dme.ErrorObject = RunCopyEntityScript(sc, srcds, destds, sc.sourceDatasourceEntityName, sc.destinationentityname, progress, token, true);
                                SendMessege(progress, token, null, sc, $"Error in Coping Data for Entity  {sc.destinationentityname}");
                            }
                        }
                        else
                        {
                            _dme.ErrorObject.Flag = Errors.Failed;
                            _dme.ErrorObject.Message = $" Could not Connect to on the Data Dource  {sc.sourcedatasourcename}";
                            _etl.errorcount = (int)_etl.StopErrorCount;
                            SendMessege(progress, token, null, sc);
                        }
                    }
                }
            }
            return _dme.ErrorObject;
        }

        /// <summary>
        /// Sends progress messages based on error status
        /// </summary>
        public void SendMessege(IProgress<PassedArgs> progress, CancellationToken token, EntityStructure refentity = null, ETLScriptDet sc = null, string messege = null)
        {
            if (_dme.ErrorObject.Flag == Errors.Failed)
            {
                SyncErrorsandTracking tr = new SyncErrorsandTracking();
                _etl.errorcount++;
                tr.errormessage = _dme.ErrorObject.Message;

                tr.rundate = DateTime.Now;
                tr.sourceEntityName = refentity?.EntityName;
                tr.currenrecordindex = _etl.CurrentScriptRecord;
                tr.sourceDataSourceName = refentity?.DataSourceID;
                if (sc != null)
                {
                    tr.parentscriptid = sc.ID;
                    sc.Tracking.Add(tr);
                }

                _etl.LoadDataLogs.Add(new LoadDataLogResult() { InputLine = $"Failed   {_etl.CurrentScriptRecord} -{messege} : {tr.errormessage}" });
                if (progress != null)
                {
                    PassedArgs ps = new PassedArgs { EventType = "Update", ParameterInt1 = _etl.CurrentScriptRecord, ParameterInt2 = _etl.ScriptCount, Messege = _dme.ErrorObject.Message };
                    progress.Report(ps);
                }
                if (_etl.errorcount > _etl.StopErrorCount)
                {
                    _etl.stoprun = true;
                    PassedArgs ps = new PassedArgs { EventType = "Stop", ParameterInt1 = _etl.CurrentScriptRecord, ParameterInt2 = _etl.ScriptCount, Messege = _dme.ErrorObject.Message };
                    progress.Report(ps);
                }
            }
            else
            {
                _etl.LoadDataLogs.Add(new LoadDataLogResult() { InputLine = $"{messege} " });
                if (progress != null)
                {
                    PassedArgs ps = new PassedArgs { EventType = "Update", ParameterInt1 = _etl.CurrentScriptRecord, ParameterInt2 = _etl.ScriptCount, Messege = _dme.ErrorObject.Message };
                    progress.Report(ps);
                }
            }
        }

        /// <summary>
        /// Runs copy entity script (delegated from copy helper for better organization)
        /// </summary>
        private IErrorsInfo RunCopyEntityScript(ETLScriptDet sc, IDataSource sourceds, IDataSource destds, string srcentity, string destentity, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            // Delegate to copy helper for actual implementation
            var copyHelper = new ETLCopyHelper(_dme, _etl);
            return copyHelper.RunCopyEntityScript(sc, sourceds, destds, srcentity, destentity, progress, token, CreateMissingEntity, map_DTL);
        }
    }
}