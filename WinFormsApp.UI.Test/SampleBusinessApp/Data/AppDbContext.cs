using System.IO;
using TheTechIdea.Beep;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Data
{
    public static class AppDbContext
    {
        public const string DataSourceName = "SampleBusinessAppDb";
        public const string DbFile = "sample_app.db";

        public static ConnectionProperties CreateSqliteConnectionProps()
        {
            // Use absolute path to avoid relative working directory issues
            var dbFullPath = Path.Combine(AppContext.BaseDirectory, DbFile);

            return new ConnectionProperties
            {
                ConnectionName = DataSourceName,
                DatabaseType = DataSourceType.SqlLite,
                Category = DatasourceCategory.RDBMS,
                DriverName = "SqliteDatasourceCore",
                DriverVersion = "1.0.0",
                ConnectionString = $"Data Source={dbFullPath};Version=3;"
            };
        }

        public static IDataSource EnsureSqliteDataSource(IDMEEditor editor)
        {
            var existing = editor.GetDataSource(DataSourceName);
            if (existing != null)
            {
                return existing;
            }

            var props = CreateSqliteConnectionProps();
            editor.ConfigEditor.AddDataConnection(props);
            var ds = editor.GetDataSource(DataSourceName);
            return ds;
        }
    }
}
