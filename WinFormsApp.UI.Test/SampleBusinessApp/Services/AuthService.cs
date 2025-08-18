using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Services
{
    public class AuthService
    {
        private readonly IDMEEditor _editor;
        
        public AuthService(IDMEEditor editor)
        {
            _editor = editor;
        }

        public bool Login(string username, string password)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor);
                ds.Openconnection();
                
                var hash = ComputeHash(password);
                var sql = $"SELECT COUNT(*) FROM Users WHERE Username = '{username.Replace("'", "''")}' AND PasswordHash = '{hash}'";
                var result = ds.GetScalar(sql);
                
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("AuthService", $"Login failed: {ex.Message}", DateTime.Now, -1, null, Errors.Failed);
                return false;
            }
        }

        public bool ValidateUser(string username)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor);
                ds.Openconnection();
                
                var sql = $"SELECT COUNT(*) FROM Users WHERE Username = '{username.Replace("'", "''")}'";
                var result = ds.GetScalar(sql);
                
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("AuthService", $"User validation failed: {ex.Message}", DateTime.Now, -1, null, Errors.Failed);
                return false;
            }
        }

        public bool CreateUser(string username, string password)
        {
            try
            {
                // Check if user already exists
                if (ValidateUser(username))
                {
                    return false; // User already exists
                }

                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor);
                ds.Openconnection();
                
                var hash = ComputeHash(password);
                var sql = $"INSERT INTO Users (Username, PasswordHash) VALUES ('{username.Replace("'", "''")}', '{hash}')";
                ds.ExecuteSql(sql);
                
                return true;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("AuthService", $"User creation failed: {ex.Message}", DateTime.Now, -1, null, Errors.Failed);
                return false;
            }
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                // First verify the old password
                if (!Login(username, oldPassword))
                {
                    return false;
                }

                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor);
                ds.Openconnection();
                
                var newHash = ComputeHash(newPassword);
                var sql = $"UPDATE Users SET PasswordHash = '{newHash}' WHERE Username = '{username.Replace("'", "''")}'";
                ds.ExecuteSql(sql);
                
                return true;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("AuthService", $"Password change failed: {ex.Message}", DateTime.Now, -1, null, Errors.Failed);
                return false;
            }
        }

        public static string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
