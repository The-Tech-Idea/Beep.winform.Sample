using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheTechIdea.Beep.Logger;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.DriversConfigurations;
using TheTechIdea.Beep.Helpers;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;

namespace TheTechIdea.Beep.FileManager
{
    /// <summary>
    /// Lightweight file-based IDataConnection implementation.
    /// This class focuses on robust path handling, consistent logging and safe operations across platforms.
    /// </summary>
    public class FileConnection : IDataConnection
    {
        public FileConnection(IDMEEditor pDMEEditor)
        {
            DMEEditor = pDMEEditor ?? throw new ArgumentNullException(nameof(pDMEEditor));
            ConnectionProp ??= new ConnectionProperties();
        }

        public bool InMemory { get; set; } = false;
        public IConnectionProperties ConnectionProp { get; set; } = new ConnectionProperties();
        public ConnectionDriversConfig DataSourceDriver { get; set; }
        public ConnectionState ConnectionStatus { get; set; } = ConnectionState.Closed;
        public int ID { get; set; }
        public string GuidID { get; set; } = Guid.NewGuid().ToString();
        public IDMEEditor DMEEditor { get; set; }
        public IDMLogger Logger { get; set; }
        public IErrorsInfo ErrorObject { get; set; }
        public IDbConnection DbConn { get; set; }

        /// <summary>
        /// Open connection (synchronous). Internally calls OpenConn which contains robust logic.
        /// </summary>
        public ConnectionState OpenConnection()
        {
            try
            {
                return OpenConn();
            }
            catch (Exception ex)
            {
                try { DMEEditor?.AddLogMessage("Error", $"OpenConnection failed: {ex.Message}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed); } catch { }
                ConnectionStatus = ConnectionState.Broken;
                return ConnectionStatus;
            }
        }

        /// <summary>
        /// Asynchronous open connection to support non-blocking file checks on UI thread.
        /// </summary>
        public async Task<ConnectionState> OpenConnectionAsync()
        {
            try
            {
                return await Task.Run(() => OpenConn());
            }
            catch (Exception ex)
            {
                try { DMEEditor?.AddLogMessage("Error", $"OpenConnectionAsync failed: {ex.Message}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed); } catch { }
                ConnectionStatus = ConnectionState.Broken;
                return ConnectionStatus;
            }
        }

        /// <summary>
        /// Replace placeholders (delegate to ConnectionHelper) and return the resolved path or connection string.
        /// </summary>
        public string ReplaceValueFromConnectionString()
        {
            if (DMEEditor == null) return null;
            try
            {
                return ConnectionHelper.ReplaceValueFromConnectionString(DataSourceDriver, ConnectionProp, DMEEditor);
            }
            catch (Exception ex)
            {
                try { DMEEditor?.AddLogMessage("Error", $"ReplaceValueFromConnectionString failed: {ex.Message}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed); } catch { }
                return null;
            }
        }

        private ConnectionState OpenConn()
        {
            if (DMEEditor == null)
            {
                throw new InvalidOperationException("DMEEditor is required");
            }

            try
            {
                // Link drivers if possible
                try
                {
                    DataSourceDriver = ConnectionHelper.LinkConnection2Drivers(ConnectionProp, DMEEditor.ConfigEditor);
                }
                catch { /* non-fatal */ }

                // Resolve candidate path
                string resolved = ReplaceValueFromConnectionString();
                string fullPath = TryResolveFilePath(resolved);

                if (string.IsNullOrEmpty(fullPath))
                {
                    DMEEditor?.AddLogMessage("Error", $"No file path could be resolved for connection {ConnectionProp?.ConnectionName ?? ConnectionProp?.FileName}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed);
                    ConnectionStatus = ConnectionState.Closed;
                    return ConnectionStatus;
                }

                if (File.Exists(fullPath))
                {
                    DMEEditor?.AddLogMessage("Success", $"Found file: {fullPath}", DateTime.Now, 0, ConnectionProp?.FileName, Errors.Ok);
                    ConnectionStatus = ConnectionState.Open;
                }
                else
                {
                    DMEEditor?.AddLogMessage("Fail", $"File not found: {fullPath}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed);
                    ConnectionStatus = ConnectionState.Broken;
                }

                return ConnectionStatus;
            }
            catch (Exception ex)
            {
                DMEEditor?.AddLogMessage("Error", $"OpenConn exception: {ex.Message}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed);
                ConnectionStatus = ConnectionState.Broken;
                return ConnectionStatus;
            }
        }

        /// <summary>
        /// Attempts to resolve a full file path from provided inputs. Handles connection string, url and file path/name.
        /// </summary>
        /// <param name="candidate">Candidate string from ReplaceValueFromConnectionString()</param>
        /// <returns>Full local file path if resolvable; otherwise null.</returns>
        private string TryResolveFilePath(string candidate)
        {
            try
            {
                // If candidate is null, try to build from ConnectionProp
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    if (!string.IsNullOrWhiteSpace(ConnectionProp?.FilePath) && !string.IsNullOrWhiteSpace(ConnectionProp?.FileName))
                    {
                        return Path.GetFullPath(Path.Combine(ConnectionProp.FilePath, ConnectionProp.FileName));
                    }
                    if (!string.IsNullOrWhiteSpace(ConnectionProp?.FileName))
                    {
                        // Try application base or configured data folder
                        var cfgPath = DMEEditor?.ConfigEditor?.CreateFileExtensionString(); // not ideal but fallback
                        string candidate2 = Path.Combine(AppContext.BaseDirectory, ConnectionProp.FileName);
                        if (File.Exists(candidate2)) return Path.GetFullPath(candidate2);
                        return null;
                    }
                    return null;
                }

                // If candidate looks like a connection string, try to parse {File} replacement already applied by ConnectionHelper
                // Candidate might be a path or full connection string. If path-like, return normalized
                // Detect url scheme
                if (candidate.IndexOfAny(new char[] { ':', '\\' }) >= 0 || candidate.StartsWith("/"))
                {
                    // Try to extract path from connection string if necessary
                    if (candidate.Contains(";"))
                    {
                        // Try common keys
                        var parts = candidate.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var p in parts)
                        {
                            var kv = p.Split(new[] { '=' }, 2);
                            if (kv.Length == 2)
                            {
                                var key = kv[0].Trim().ToLower();
                                var val = kv[1].Trim().Trim('\'','\"');
                                if (key.Contains("file") || key.Contains("data source") || key.Contains("datasource") || key.Contains("data") || key.Contains("path"))
                                {
                                    try
                                    {
                                        string tryPath = val;
                                        if (!Path.IsPathRooted(tryPath)) tryPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, tryPath));
                                        return tryPath;
                                    }
                                    catch { }
                                }
                            }
                        }
                    }

                    // Treat candidate as direct path
                    try
                    {
                        string maybe = candidate.Trim('"');
                        if (!Path.IsPathRooted(maybe)) maybe = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, maybe));
                        return maybe;
                    }
                    catch { return null; }
                }

                // Fallback: append candidate to base directory
                var fallback = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, candidate));
                return fallback;
            }
            catch
            {
                return null;
            }
        }

        public ConnectionState OpenConnection(DataSourceType dbtype, string host, int port, string database, string userid, string password, string parameters)
        {
            // This connection is file-based; the parameterized overload is not applicable.
            return OpenConnection();
        }

        public ConnectionState OpenConnection(DataSourceType dbtype, string connectionstring)
        {
            // Same as above: set connection string and call OpenConnection
            ConnectionProp.ConnectionString = connectionstring;
            return OpenConnection();
        }

        /// <summary>
        /// Close the file connection. This implementation only validates file presence and updates status.
        /// </summary>
        public virtual ConnectionState CloseConn()
        {
            try
            {
                string full = TryResolveFilePath(ReplaceValueFromConnectionString());
                if (!string.IsNullOrEmpty(full) && File.Exists(full))
                {
                    DMEEditor?.AddLogMessage("Success", $"Closed Connection for File {ConnectionProp.FileName}", DateTime.Now, 0, ConnectionProp.FileName, Errors.Ok);
                    ConnectionStatus = ConnectionState.Closed;
                }
                else
                {
                    DMEEditor?.AddLogMessage("Warning", $"Could not find file to close: {ConnectionProp?.FileName ?? full}", DateTime.Now, 0, ConnectionProp?.FileName, Errors.Failed);
                    ConnectionStatus = ConnectionState.Broken;
                }
            }
            catch (Exception ex)
            {
                DMEEditor?.AddLogMessage("Error", $"CloseConn exception: {ex.Message}", DateTime.Now, -1, ConnectionProp?.FileName, Errors.Failed);
                ConnectionStatus = ConnectionState.Broken;
            }
            return ConnectionStatus;
        }
    }
}
