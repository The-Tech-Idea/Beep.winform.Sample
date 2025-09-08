using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Report;
using TheTechIdea.Beep.Workflow;
using TheTechIdea.Beep.Workflow.Mapping;
using TheTechIdea.Beep.Utilities;
using System.IO;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;
using System.ComponentModel;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Rules;

namespace TheTechIdea.Beep.Editor.ETL
{
    /// <summary>
    /// Represents an Extract, Transform, Load (ETL) process.
    /// Refactored to use helper classes for better organization and maintainability.
    /// </summary>
    public class ETLEditor : IETL
    {
        /// <summary>
        /// Initializes a new instance of the ETL class.
        /// </summary>
        /// <param name="_DMEEditor">The DME editor to use for the ETL process.</param>
        public ETLEditor(IDMEEditor _DMEEditor)
        {
            DMEEditor = _DMEEditor ?? throw new ArgumentNullException(nameof(_DMEEditor));
            
            // Initialize helpers
            _scriptHelper = new ETLScriptHelper(DMEEditor, this);
            _copyHelper = new ETLCopyHelper(DMEEditor, this);
            _executionHelper = new ETLExecutionHelper(DMEEditor, this);
        }

        // Helper instances
        private readonly ETLScriptHelper _scriptHelper;
        private readonly ETLCopyHelper _copyHelper;
        private readonly ETLExecutionHelper _executionHelper;

        /// <summary>
        /// Event that is raised when a process is passed.
        /// </summary>
        public event EventHandler<PassedArgs> PassEvent;

        /// <summary>Gets or sets the DMEEditor instance.</summary>
        public IDMEEditor DMEEditor { get { return _DMEEditor; } set { _DMEEditor = value; } }

        /// <summary>Gets or sets the rules editor.</summary>
        public IRuleEngine RulesEngine { get; set; }

        /// <summary>Gets or sets the PassedArgs object.</summary>
        public PassedArgs Passedargs { get; set; }

        /// <summary>Gets or sets the count of scripts.</summary>
        public int ScriptCount { get; set; }

        /// <summary>Gets or sets the current script record.</summary>
        public int CurrentScriptRecord { get; set; }

        /// <summary>Gets or sets the stop error count.</summary>
        public decimal StopErrorCount { get; set; } = 10;

        /// <summary>Gets or sets the list of loaded data logs.</summary>
        public List<LoadDataLogResult> LoadDataLogs { get; set; } = new List<LoadDataLogResult>();

        /// <summary>Gets or sets the ETL script for HDR processing.</summary>
        public ETLScriptHDR Script { get; set; } = new ETLScriptHDR();

        #region "Local Variables"
        internal bool stoprun = false;
        private IDMEEditor _DMEEditor;
        internal int errorcount = 0;
        private List<DefaultValue> CurrrentDBDefaults = new List<DefaultValue>();
        private bool disposedValue;
        #endregion

        #region "Create Scripts"
        public void CreateScriptHeader(IDataSource Srcds, IProgress<PassedArgs> progress, CancellationToken token)
        {
            int i = 0;
            Script = new ETLScriptHDR();
            Script.scriptSource = Srcds.DatasourceName;
            List<EntityStructure> ls = new List<EntityStructure>();
            Srcds.GetEntitesList();
            foreach (string item in Srcds.EntitiesNames)
            {
                ls.Add(Srcds.GetEntityStructure(item, true));
            }
            Script.ScriptDTL = GetCreateEntityScript(Srcds, ls, progress, token);
            foreach (var item in ls)
            {
                ETLScriptDet upscript = new ETLScriptDet();
                upscript.sourcedatasourcename = item.DataSourceID;
                upscript.sourceentityname = item.EntityName;
                upscript.sourceDatasourceEntityName = item.EntityName;
                upscript.destinationDatasourceEntityName = item.EntityName;
                upscript.destinationentityname = item.EntityName;
                upscript.destinationdatasourcename = Srcds.DatasourceName;
                upscript.scriptType = DDLScriptType.CopyData;
                Script.ScriptDTL.Add(upscript);
                i += 1;
            }
        }

        public List<ETLScriptDet> GetCreateEntityScript(IDataSource ds, List<string> entities, IProgress<PassedArgs> progress, CancellationToken token, bool copydata = false)
        {
            return _scriptHelper.GetCreateEntityScript(ds, entities, progress, token, copydata);
        }

        public List<ETLScriptDet> GetCreateEntityScript(IDataSource Dest, List<EntityStructure> entities, IProgress<PassedArgs> progress, CancellationToken token, bool copydata = false)
        {
            return _scriptHelper.GetCreateEntityScript(Dest, entities, progress, token, copydata);
        }

        public List<ETLScriptDet> GetCopyDataEntityScript(IDataSource Dest, List<EntityStructure> entities, IProgress<PassedArgs> progress, CancellationToken token)
        {
            return _scriptHelper.GetCopyDataEntityScript(Dest, entities, progress, token);
        }
        #endregion

        #region "Copy Data"
        public IErrorsInfo CopyEntitiesStructure(IDataSource sourceds, IDataSource destds, List<string> entities, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true)
        {
            try
            {
                var ls = from e in sourceds.Entities
                         from r in entities
                         where e.EntityName == r
                         select e;
                foreach (EntityStructure item in ls)
                {
                    CopyEntityStructure(sourceds, destds, item.EntityName, item.EntityName, progress, token, CreateMissingEntity);
                }
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Error copying Data ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return DMEEditor.ErrorObject;
        }

        public IErrorsInfo CopyEntityStructure(IDataSource sourceds, IDataSource destds, string srcentity, string destentity, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true)
        {
            try
            {
                EntityStructure item = sourceds.GetEntityStructure(srcentity, true);
                if (item != null)
                {
                    if (destds.Category == DatasourceCategory.RDBMS)
                    {
                        IRDBSource rDB = (IRDBSource)destds;
                        rDB.DisableFKConstraints(item);
                    }
                    if (destds.CreateEntityAs(item))
                    {
                        DMEEditor.AddLogMessage("Success", $"Creating Entity  {item.EntityName} on {destds.DatasourceName}", DateTime.Now, 0, null, Errors.Ok);
                    }
                    else
                    {
                        DMEEditor.AddLogMessage("Fail", $"Error : Could not Create  Entity {item.EntityName} on {destds.DatasourceName}", DateTime.Now, 0, null, Errors.Failed);
                    }
                    if (destds.Category == DatasourceCategory.RDBMS)
                    {
                        IRDBSource rDB = (IRDBSource)destds;
                        rDB.EnableFKConstraints(item);
                    }
                }
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Error Could not Create  Entity {srcentity} on {destds.DatasourceName} ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return DMEEditor.ErrorObject;
        }

        public IErrorsInfo CopyDatasourceData(IDataSource sourceds, IDataSource destds, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            try
            {
                foreach (EntityStructure item in sourceds.Entities)
                {
                    CopyEntityData(sourceds, destds, item.EntityName, item.EntityName, progress, token, CreateMissingEntity);
                }
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Error copying Data ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return DMEEditor.ErrorObject;
        }

        public IErrorsInfo CopyEntitiesData(IDataSource sourceds, IDataSource destds, List<string> entities, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            try
            {
                var ls = from e in sourceds.Entities
                         from r in entities
                         where e.EntityName == r
                         select e;

                foreach (EntityStructure item in ls)
                {
                    if (item.EntityName != item.DatasourceEntityName && !string.IsNullOrEmpty(item.DatasourceEntityName))
                    {
                        CopyEntityData(sourceds, destds, item.DatasourceEntityName, item.EntityName, progress, token, CreateMissingEntity);
                    }
                    else
                        CopyEntityData(sourceds, destds, item.EntityName, item.EntityName, progress, token, CreateMissingEntity);

                }
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Error copying Data ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return DMEEditor.ErrorObject;
        }

        public IErrorsInfo CopyEntityData(IDataSource sourceds, IDataSource destds, string srcentity, string destentity, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            return _copyHelper.CopyEntityData(sourceds, destds, srcentity, destentity, progress, token, CreateMissingEntity, map_DTL);
        }

        public IErrorsInfo CopyEntitiesData(IDataSource sourceds, IDataSource destds, List<ETLScriptDet> scripts, IProgress<PassedArgs> progress, CancellationToken token, bool CreateMissingEntity = true, EntityDataMap_DTL map_DTL = null)
        {
            try
            {
                string srcentityname = "";
                foreach (ETLScriptDet s in scripts.Where(i => i.scriptType == DDLScriptType.CopyData))
                {
                    if (s.sourceentityname != s.sourceDatasourceEntityName && !string.IsNullOrEmpty(s.sourceDatasourceEntityName))
                    {
                        srcentityname = s.sourceDatasourceEntityName;
                    }
                    else
                        srcentityname = s.sourceentityname;
                    CopyEntityData(sourceds, destds, srcentityname, s.sourceentityname, progress, token, CreateMissingEntity);
                }
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Error copying Data ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return DMEEditor.ErrorObject;
        }
        #endregion

        #region "Run Scripts"
        public async Task<IErrorsInfo> RunChildScriptAsync(ETLScriptDet ParentScript, IDataSource srcds, IDataSource destds, IProgress<PassedArgs> progress, CancellationToken token)
        {
            return await _executionHelper.RunChildScriptAsync(ParentScript, srcds, destds, progress, token);
        }

        public async Task<IErrorsInfo> RunCreateScript(IProgress<PassedArgs> progress, CancellationToken token, bool copydata = true, bool useEntityStructure = true)
        {
            int numberToCompute = 0;

            IDataSource destds = null;
            IDataSource srcds = null;
            LoadDataLogs = new List<LoadDataLogResult>();
            numberToCompute = DMEEditor.ETL.Script.ScriptDTL.Count();
            List<ETLScriptDet> crls = DMEEditor.ETL.Script.ScriptDTL.Where(i => i.scriptType == DDLScriptType.CreateEntity).ToList();
            List<ETLScriptDet> copudatals = DMEEditor.ETL.Script.ScriptDTL.Where(i => i.scriptType == DDLScriptType.CopyData).ToList();
            List<ETLScriptDet> AlterForls = DMEEditor.ETL.Script.ScriptDTL.Where(i => i.scriptType == DDLScriptType.AlterFor).ToList();
            numberToCompute = DMEEditor.ETL.Script.ScriptDTL.Count;
            int p1 = DMEEditor.ETL.Script.ScriptDTL.Where(u => u.scriptType == DDLScriptType.CreateEntity).Count();
            ScriptCount = p1;
            CurrentScriptRecord = 0;
            errorcount = 0;
            stoprun = false;
            bool CreateSuccess;
            EntityStructure entitystr;
            foreach (ETLScriptDet sc in DMEEditor.ETL.Script.ScriptDTL.OrderBy(p => p.ID))
            {
                CreateSuccess = true;
                destds = DMEEditor.GetDataSource(sc.destinationdatasourcename);
                srcds = DMEEditor.GetDataSource(sc.sourcedatasourcename);
                CurrentScriptRecord += 1;
                if (errorcount == StopErrorCount)
                {
                    return DMEEditor.ErrorObject;
                }
                if (destds != null)
                {
                    DMEEditor.OpenDataSource(sc.destinationdatasourcename);
                    if (stoprun == false)
                    {
                        if (destds.ConnectionStatus == ConnectionState.Open)
                        {
                            switch (sc.scriptType)
                            {
                                case DDLScriptType.CreateEntity:
                                    if (!useEntityStructure || sc.SourceEntity == null)
                                    {
                                        entitystr = (EntityStructure)srcds.GetEntityStructure(sc.sourceDatasourceEntityName, false).Clone();
                                    }
                                    else
                                    {
                                        entitystr = sc.SourceEntity;
                                    }

                                    if (sc.sourceDatasourceEntityName != sc.destinationentityname)
                                    {
                                        entitystr.EntityName = sc.destinationentityname;
                                        entitystr.DatasourceEntityName = sc.destinationentityname;
                                        entitystr.OriginalEntityName = sc.destinationentityname;
                                    }

                                    _executionHelper.SendMessege(progress, token, entitystr, sc, $"Creating Entity  {entitystr.EntityName} ");
                                    bool retval = destds.CreateEntityAs(entitystr);
                                    if (retval)
                                    {
                                        _executionHelper.SendMessege(progress, token, entitystr, sc, $"Successfully Created Entity  {entitystr.EntityName} ");
                                        sc.Active = true;
                                        sc.IsCreated = true;
                                        sc.Active = true;
                                        if (sc.CopyDataScripts.Count > 0 && sc.CopyData && sc.IsCreated)
                                        {
                                            _executionHelper.SendMessege(progress, token, entitystr, sc, $"Started  Coping Data From {entitystr.EntityName} ");
                                            var t = await RunChildScriptAsync(sc, srcds, destds, progress, token);
                                            CreateSuccess = true;
                                        }
                                    }
                                    else
                                    {
                                        DMEEditor.ErrorObject.Flag = Errors.Failed;
                                        DMEEditor.ErrorObject.Message = $"Failed in Creating Entity   {entitystr.EntityName} ";
                                        _executionHelper.SendMessege(progress, token, entitystr, sc, $"Failed in Creating Entity   {entitystr.EntityName} ");
                                        sc.Active = false;
                                        sc.Failed = true;
                                        CreateSuccess = false;
                                    }
                                    break;
                                case DDLScriptType.CopyData:
                                    if (CreateSuccess == false)
                                    {
                                        _executionHelper.SendMessege(progress, token, null, sc, $"Cannot Copy Data for Failed  Entity   {sc.destinationentityname} ");
                                        break;
                                    }
                                    _executionHelper.SendMessege(progress, token, null, sc, $"Started Coping Data for Entity  {sc.destinationentityname}  in {sc.destinationdatasourcename}");

                                    await Task.Run(() =>
                                    {
                                        DMEEditor.ErrorObject = _copyHelper.RunCopyEntityScript(sc, srcds, destds, sc.sourceDatasourceEntityName, sc.destinationentityname, progress, token, true);
                                    });
                                    _executionHelper.SendMessege(progress, token, null, sc, $"Finished in Coping Data for Entity  {sc.destinationentityname}");
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            DMEEditor.ErrorObject.Flag = Errors.Failed;
                            DMEEditor.ErrorObject.Message = $" Could not Connect to on the Data Dources {sc.destinationdatasourcename} or {sc.sourcedatasourcename}";
                            _executionHelper.SendMessege(progress, token, null, sc);
                        }
                    }
                }
            }
            return DMEEditor.ErrorObject;
        }

        #endregion

        #region "Import Methods"
        public IErrorsInfo CreateImportScript(EntityDataMap mapping, EntityDataMap_DTL SelectedMapping)
        {
            try
            {
                Script = new ETLScriptHDR();
                Script.scriptSource = SelectedMapping.EntityDataSource;
                errorcount = 0;
                ScriptCount = 0;
                LoadDataLogs.Clear();
                Script.ScriptDTL.Add(new ETLScriptDet() { Active = true, destinationdatasourcename = mapping.EntityDataSource, destinationDatasourceEntityName = mapping.EntityName, destinationentityname = mapping.EntityName, scriptType = DDLScriptType.CopyData, Mapping = SelectedMapping, sourcedatasourcename = SelectedMapping.EntityDataSource, sourceDatasourceEntityName = SelectedMapping.EntityName, sourceentityname = SelectedMapping.EntityName });
                DMEEditor.AddLogMessage("OK", $"Generated Copy Data script", DateTime.Now, -1, "CopyDatabase", Errors.Ok);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Error Generating Copy Data script ({ex.Message})", DateTime.Now, -1, "CopyDatabase", Errors.Failed);
            }
            return DMEEditor.ErrorObject;
        }

        public async Task<IErrorsInfo> RunImportScript(IProgress<PassedArgs> progress, CancellationToken token, bool useEntityStructure = true)
        {
            IDataSource destds = null;
            IDataSource srcds = null;
            ScriptCount = 1;
            CurrentScriptRecord = 0;
            errorcount = 0;
            stoprun = false;
            EntityStructure entitystr;
            CurrentScriptRecord += 1;
            LoadDataLogs = new List<LoadDataLogResult>();
            ETLScriptDet sc = DMEEditor.ETL.Script.ScriptDTL.FirstOrDefault();
            if (sc != null)
            {
                destds = DMEEditor.GetDataSource(sc.destinationdatasourcename);
                srcds = DMEEditor.GetDataSource(sc.sourcedatasourcename);
                if (errorcount == StopErrorCount)
                {
                    return DMEEditor.ErrorObject;
                }
                if (destds != null)
                {

                    DMEEditor.OpenDataSource(sc.destinationdatasourcename);
                    if (stoprun == false)
                    {
                        if (destds.ConnectionStatus == ConnectionState.Open)
                        {
                            if (sc.scriptType == DDLScriptType.CopyData)
                            {
                                CurrrentDBDefaults = DMEEditor.ConfigEditor.DataConnections[DMEEditor.ConfigEditor.DataConnections.FindIndex(i => i.ConnectionName == destds.DatasourceName)].DatasourceDefaults;
                                if (!useEntityStructure || sc.SourceEntity == null)
                                {
                                    entitystr = (EntityStructure)srcds.GetEntityStructure(sc.sourceDatasourceEntityName, false).Clone();
                                }
                                else
                                {
                                    entitystr = sc.SourceEntity;
                                }

                                sc.errormessage = DMEEditor.ErrorObject.Message;

                                sc.Active = false;
                                _executionHelper.SendMessege(progress, token, null, sc, "Starting Import Entities Script");

                                if (errorcount == StopErrorCount)
                                {
                                    return DMEEditor.ErrorObject;
                                }
                                await Task.Run(() => { return _copyHelper.RunCopyEntityScript(sc, srcds, destds, sc.sourceentityname, sc.destinationentityname, progress, token, false, sc.Mapping); });
                            }
                        }
                        else
                        {
                            DMEEditor.ErrorObject.Flag = Errors.Failed;
                            DMEEditor.ErrorObject.Message = $" Could not Connect to on the Data Dources {sc.destinationdatasourcename} or {sc.sourcedatasourcename}";
                            _executionHelper.SendMessege(progress, token);
                        }

                    }
                }
            }
            return DMEEditor.ErrorObject;
        }
        #endregion

        // Keep remaining methods from original implementation
        // ...existing code for InsertEntity, LoadETL, SaveETL, etc...

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LoadDataLogs = null;
                    Script = null;
                    CurrrentDBDefaults = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}