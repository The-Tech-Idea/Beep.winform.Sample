using System;
using System.Collections.Generic;
using System.Linq;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Services
{
    /// <summary>
    /// Customer service for CRUD operations and business logic
    /// </summary>
    public class CustomerService
    {
        private readonly IDMEEditor _editor;
        
        public CustomerService(IDMEEditor editor)
        {
            _editor = editor;
        }

        #region Customer CRUD Operations

        public List<Customer> GetAllCustomers()
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = @"
                    SELECT ID, Name, Email, Phone, Address, City, PostalCode, Country,
                           CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms,
                           Status, CustomerType, CreatedAt, UpdatedAt, LastContactDate
                    FROM Customers 
                    ORDER BY Name";
                
                return ds?.GetData<Customer>(sql) ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetAllCustomers failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new List<Customer>();
            }
        }

        public Customer GetCustomerById(int id)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = @"
                    SELECT ID, Name, Email, Phone, Address, City, PostalCode, Country,
                           CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms,
                           Status, CustomerType, CreatedAt, UpdatedAt, LastContactDate
                    FROM Customers 
                    WHERE ID = " + id;
                
                return ds?.GetData<Customer>(sql)?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetCustomerById failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return null;
            }
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return GetAllCustomers();

                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var searchPattern = $"%{searchTerm.Replace("'", "''")}%";
                var sql = @"
                    SELECT ID, Name, Email, Phone, Address, City, PostalCode, Country,
                           CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms,
                           Status, CustomerType, CreatedAt, UpdatedAt, LastContactDate
                    FROM Customers 
                    WHERE Name LIKE '" + searchPattern + @"' 
                       OR Email LIKE '" + searchPattern + @"' 
                       OR CompanyName LIKE '" + searchPattern + @"'
                       OR Phone LIKE '" + searchPattern + @"'
                    ORDER BY Name";
                
                return ds?.GetData<Customer>(sql) ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"SearchCustomers failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new List<Customer>();
            }
        }

        public ErrorsInfo CreateCustomer(Customer customer)
        {
            try
            {
                var validation = ValidateCustomer(customer);
                if (!string.IsNullOrEmpty(validation))
                {
                    return new ErrorsInfo { Flag = Errors.Failed, Message = validation };
                }

                // Check for duplicate email
                if (IsEmailExists(customer.Email, customer.Id))
                {
                    return new ErrorsInfo { Flag = Errors.Failed, Message = "Email address already exists" };
                }

                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = @"
                    INSERT INTO Customers (
                        Name, Email, Phone, Address, City, PostalCode, Country,
                        CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms,
                        Status, CustomerType, CreatedAt
                    ) VALUES (
                        @Name, @Email, @Phone, @Address, @City, @PostalCode, @Country,
                        @CompanyName, @ContactPerson, @TaxId, @CreditLimit, @PaymentTerms,
                        @Status, @CustomerType, @CreatedAt
                    )";

                customer.CreatedAt = DateTime.UtcNow;
                ds?.SaveData(sql, customer)?.Wait();

                return new ErrorsInfo { Flag = Errors.Ok, Message = "Customer created successfully" };
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"CreateCustomer failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new ErrorsInfo { Flag = Errors.Failed, Message = $"Failed to create customer: {ex.Message}" };
            }
        }

        public ErrorsInfo UpdateCustomer(Customer customer)
        {
            try
            {
                var validation = ValidateCustomer(customer);
                if (!string.IsNullOrEmpty(validation))
                {
                    return new ErrorsInfo { Flag = Errors.Failed, Message = validation };
                }

                // Check for duplicate email (excluding current customer)
                if (IsEmailExists(customer.Email, customer.Id))
                {
                    return new ErrorsInfo { Flag = Errors.Failed, Message = "Email address already exists" };
                }

                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = @"
                    UPDATE Customers SET
                        Name = @Name, Email = @Email, Phone = @Phone, 
                        Address = @Address, City = @City, PostalCode = @PostalCode, Country = @Country,
                        CompanyName = @CompanyName, ContactPerson = @ContactPerson, TaxId = @TaxId, 
                        CreditLimit = @CreditLimit, PaymentTerms = @PaymentTerms,
                        Status = @Status, CustomerType = @CustomerType, UpdatedAt = @UpdatedAt
                    WHERE ID = @ID";

                customer.UpdatedAt = DateTime.UtcNow;
                ds?.SaveData(sql, customer)?.Wait();

                return new ErrorsInfo { Flag = Errors.Ok, Message = "Customer updated successfully" };
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"UpdateCustomer failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new ErrorsInfo { Flag = Errors.Failed, Message = $"Failed to update customer: {ex.Message}" };
            }
        }

        public ErrorsInfo DeleteCustomer(int id)
        {
            try
            {
                // Check if customer has orders or invoices
                if (HasActiveTransactions(id))
                {
                    return new ErrorsInfo 
                    { 
                        Flag = Errors.Failed, 
                        Message = "Cannot delete customer with active orders or invoices" 
                    };
                }

                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = "DELETE FROM Customers WHERE ID = @ID";
                ds?.SaveData(sql, new { Id = id })?.Wait();

                return new ErrorsInfo { Flag = Errors.Ok, Message = "Customer deleted successfully" };
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"DeleteCustomer failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new ErrorsInfo { Flag = Errors.Failed, Message = $"Failed to delete customer: {ex.Message}" };
            }
        }

        #endregion

        #region Customer Business Logic

        public ErrorsInfo UpdateLastContactDate(int customerId)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = "UPDATE Customers SET LastContactDate = @LastContactDate WHERE ID = @ID";
                ds?.SaveData(sql, new { Id = customerId, LastContactDate = DateTime.UtcNow })?.Wait();

                return new ErrorsInfo { Flag = Errors.Ok };
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"UpdateLastContactDate failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new ErrorsInfo { Flag = Errors.Failed, Message = ex.Message };
            }
        }

        public List<Customer> GetCustomersByStatus(string status)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = @"
                    SELECT ID, Name, Email, Phone, Address, City, PostalCode, Country,
                           CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms,
                           Status, CustomerType, CreatedAt, UpdatedAt, LastContactDate
                    FROM Customers 
                    WHERE Status = '" + status.Replace("'", "''") + @"'
                    ORDER BY Name";
                
                return ds?.GetData<Customer>(sql) ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetCustomersByStatus failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new List<Customer>();
            }
        }

        public List<Customer> GetCustomersByType(string customerType)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = @"
                    SELECT ID, Name, Email, Phone, Address, City, PostalCode, Country,
                           CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms,
                           Status, CustomerType, CreatedAt, UpdatedAt, LastContactDate
                    FROM Customers 
                    WHERE CustomerType = '" + customerType.Replace("'", "''") + @"'
                    ORDER BY Name";
                
                return ds?.GetData<Customer>(sql) ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetCustomersByType failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new List<Customer>();
            }
        }

        public decimal GetCustomerTotalOrders(int customerId)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = "SELECT COALESCE(SUM(TotalAmount), 0) FROM SalesOrders WHERE CustomerId = @CustomerId";
                var result = ds?.GetScalar(sql.Replace("@CustomerId", customerId.ToString()));
                
                return Convert.ToDecimal(result ?? 0);
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetCustomerTotalOrders failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return 0;
            }
        }

        public int GetCustomerOrderCount(int customerId)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = "SELECT COUNT(*) FROM SalesOrders WHERE CustomerId = @CustomerId";
                var result = ds?.GetScalar(sql.Replace("@CustomerId", customerId.ToString()));
                
                return Convert.ToInt32(result ?? 0);
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetCustomerOrderCount failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return 0;
            }
        }

        #endregion

        #region Validation and Helper Methods

        private string ValidateCustomer(Customer customer)
        {
            if (customer == null)
                return "Customer cannot be null";

            if (string.IsNullOrWhiteSpace(customer.Name))
                return "Customer name is required";

            if (string.IsNullOrWhiteSpace(customer.Email))
                return "Customer email is required";

            if (!IsValidEmail(customer.Email))
                return "Invalid email address format";

            if (customer.CreditLimit < 0)
                return "Credit limit cannot be negative";

            return string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsEmailExists(string email, int excludeId = 0)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var sql = "SELECT COUNT(*) FROM Customers WHERE Email = @Email AND ID != @ExcludeId";
                var result = ds?.GetScalar(sql.Replace("@Email", $"'{email.Replace("'", "''")}'")
                                              .Replace("@ExcludeId", excludeId.ToString()));
                
                return Convert.ToInt32(result ?? 0) > 0;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"IsEmailExists failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return false;
            }
        }

        private bool HasActiveTransactions(int customerId)
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                // Check for orders
                var orderSql = "SELECT COUNT(*) FROM SalesOrders WHERE CustomerId = @CustomerId";
                var orderResult = ds?.GetScalar(orderSql.Replace("@CustomerId", customerId.ToString()));
                
                if (Convert.ToInt32(orderResult ?? 0) > 0)
                    return true;

                // Check for invoices
                var invoiceSql = "SELECT COUNT(*) FROM Invoices WHERE CustomerId = @CustomerId";
                var invoiceResult = ds?.GetScalar(invoiceSql.Replace("@CustomerId", customerId.ToString()));
                
                return Convert.ToInt32(invoiceResult ?? 0) > 0;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"HasActiveTransactions failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return false; // If we can't check, allow deletion but log the error
            }
        }

        #endregion

        #region Statistics and Reporting

        public CustomerStats GetCustomerStatistics()
        {
            try
            {
                var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
                ds?.Openconnection();
                
                var stats = new CustomerStats();

                // Total customers
                var totalSql = "SELECT COUNT(*) FROM Customers";
                stats.TotalCustomers = Convert.ToInt32(ds?.GetScalar(totalSql) ?? 0);

                // Active customers
                var activeSql = "SELECT COUNT(*) FROM Customers WHERE Status = 'Active'";
                stats.ActiveCustomers = Convert.ToInt32(ds?.GetScalar(activeSql) ?? 0);

                // Premium customers
                var premiumSql = "SELECT COUNT(*) FROM Customers WHERE CustomerType = 'Premium'";
                stats.PremiumCustomers = Convert.ToInt32(ds?.GetScalar(premiumSql) ?? 0);

                // VIP customers
                var vipSql = "SELECT COUNT(*) FROM Customers WHERE CustomerType = 'VIP'";
                stats.VipCustomers = Convert.ToInt32(ds?.GetScalar(vipSql) ?? 0);

                return stats;
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("CustomerService", $"GetCustomerStatistics failed: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
                return new CustomerStats();
            }
        }

        #endregion
    }

    /// <summary>
    /// Customer statistics model
    /// </summary>
    public class CustomerStats
    {
        public int TotalCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int PremiumCustomers { get; set; }
        public int VipCustomers { get; set; }
        public decimal AverageCreditLimit { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}