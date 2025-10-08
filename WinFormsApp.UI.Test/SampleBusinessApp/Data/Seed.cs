using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.ConfigUtil;
using WinFormsApp.UI.Test.SampleBusinessApp.Services;
using TheTechIdea.Beep;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Data
{
    public static class Seed
    {
        public static void EnsureCreatedAndSeeded(IDMEEditor editor)
        {
            try
            {
                var ds = AppDbContext.EnsureSqliteDataSource(editor);
                ds.Openconnection();
                
                if (ds.ConnectionStatus != ConnectionState.Open)
                {
                    editor.AddLogMessage("SampleApp", "Failed to open SQLite connection", DateTime.Now, -1, null, Errors.Failed);
                    return;
                }

                // Create tables
                CreateTables(ds);
                
                // Seed data
                SeedUsers(ds);
                SeedCustomers(ds);
                SeedProducts(ds);
                SeedCategories(ds);
                SeedTasks(ds);
                SeedEvents(ds);
                
                editor.AddLogMessage("SampleApp", "Database seeded successfully", DateTime.Now, -1, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                editor.AddLogMessage("SampleApp", $"Seeding failed: {ex.Message}", DateTime.Now, -1, null, Errors.Failed);
            }
        }

        private static void CreateTables(IDataSource ds)
        {
            // Users table
            ds.ExecuteSql(@"
                CREATE TABLE IF NOT EXISTS Users (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Email TEXT,
                    FullName TEXT,
                    Role TEXT DEFAULT 'User',
                    CreatedAt TEXT NOT NULL,
                    LastLogin TEXT
                )");

            // Customers table
            ds.ExecuteSql(@"
                CREATE TABLE IF NOT EXISTS Customers (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Phone TEXT,
                    Address TEXT,
                    City TEXT,
                    PostalCode TEXT,
                    Country TEXT,
                    CompanyName TEXT,
                    ContactPerson TEXT,
                    TaxId TEXT,
                    CreditLimit REAL DEFAULT 0,
                    PaymentTerms TEXT DEFAULT 'Net 30',
                    Status TEXT DEFAULT 'Active',
                    CustomerType TEXT DEFAULT 'Standard',
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT,
                    LastContactDate TEXT
                )");

            // Categories table
            ds.ExecuteSql(@"
                CREATE TABLE IF NOT EXISTS Categories (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Color TEXT,
                    Icon TEXT,
                    CreatedAt TEXT NOT NULL
                )");

            // Products table (enhanced)
            ds.ExecuteSql(@"
                CREATE TABLE IF NOT EXISTS Products (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Price REAL NOT NULL,
                    Stock INTEGER NOT NULL,
                    CategoryId INTEGER,
                    SKU TEXT UNIQUE,
                    Rating REAL DEFAULT 0.0,
                    ImagePath TEXT,
                    IsActive INTEGER DEFAULT 1,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT,
                    FOREIGN KEY (CategoryId) REFERENCES Categories(ID)
                )");

            // Tasks table
            ds.ExecuteSql(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status TEXT DEFAULT 'Pending',
                    Priority TEXT DEFAULT 'Medium',
                    AssignedTo TEXT,
                    DueDate TEXT,
                    CreatedAt TEXT NOT NULL
                )");

            // Invoices table (basic)
            ds.ExecuteSql(@"
                CREATE TABLE IF NOT EXISTS Invoices (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    InvoiceNumber TEXT NOT NULL,
                    OrderId INTEGER,
                    CustomerId INTEGER NOT NULL,
                    InvoiceDate TEXT NOT NULL,
                    DueDate TEXT,
                    SubTotal REAL DEFAULT 0,
                    TaxAmount REAL DEFAULT 0,
                    TotalAmount REAL DEFAULT 0,
                    PaidAmount REAL DEFAULT 0,
                    BalanceAmount REAL DEFAULT 0,
                    Status TEXT DEFAULT 'Draft',
                    PaymentStatus TEXT DEFAULT 'Unpaid',
                    Notes TEXT,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT
                )");
        }

        private static void SeedUsers(IDataSource ds)
        {
            var userCount = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Users"));
            if (userCount == 0)
            {
                var now = DateTime.UtcNow.ToString("o");
                var adminHash = AuthService.ComputeHash("admin");
                var userHash = AuthService.ComputeHash("user123");

                ds.ExecuteSql($@"
                    INSERT INTO Users (Username, PasswordHash, Email, FullName, Role, CreatedAt) VALUES 
                    ('admin', '{adminHash}', 'admin@samplecorp.com', 'System Administrator', 'Admin', '{now}'),
                    ('john.doe', '{userHash}', 'john.doe@samplecorp.com', 'John Doe', 'User', '{now}'),
                    ('jane.smith', '{userHash}', 'jane.smith@samplecorp.com', 'Jane Smith', 'Manager', '{now}')
                ");
            }
        }

        private static void SeedCategories(IDataSource ds)
        {
            var categoryCount = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Categories"));
            if (categoryCount == 0)
            {
                var now = DateTime.UtcNow.ToString("o");
                ds.ExecuteSql($@"
                    INSERT INTO Categories (Name, Description, Color, Icon, CreatedAt) VALUES 
                    ('Electronics', 'Electronic devices and accessories', '#007bff', 'electronics.svg', '{now}'),
                    ('Office Supplies', 'Office equipment and supplies', '#28a745', 'office.svg', '{now}'),
                    ('Software', 'Software products and licenses', '#ffc107', 'software.svg', '{now}'),
                    ('Books', 'Books and educational materials', '#dc3545', 'books.svg', '{now}'),
                    ('Furniture', 'Office and home furniture', '#6c757d', 'furniture.svg', '{now}')
                ");
            }
        }

        private static void SeedProducts(IDataSource ds)
        {
            var productCount = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Products"));
            if (productCount == 0)
            {
                var now = DateTime.UtcNow.ToString("o");
                ds.ExecuteSql($@"
                    INSERT INTO Products (Name, Description, Price, Stock, CategoryId, SKU, Rating, CreatedAt) VALUES 
                    ('Wireless Keyboard', 'Ergonomic wireless keyboard with backlight', 79.99, 25, 1, 'KB-001', 4.5, '{now}'),
                    ('Optical Mouse', 'High-precision optical mouse', 39.49, 40, 1, 'MS-001', 4.2, '{now}'),
                    ('27 Monitor', 'IPS display with 4K resolution', 299.00, 10, 1, 'MN-001', 4.8, '{now}'),
                    ('Office Chair', 'Ergonomic office chair with lumbar support', 199.99, 15, 5, 'CH-001', 4.3, '{now}'),
                    ('Desk Lamp', 'LED desk lamp with adjustable brightness', 45.00, 30, 2, 'DL-001', 4.1, '{now}'),
                    ('Notebook Set', 'Premium notebook set with pen', 24.99, 50, 2, 'NB-001', 4.0, '{now}'),
                    ('Project Software', 'Project management software license', 99.99, 100, 3, 'SW-001', 4.6, '{now}'),
                    ('Programming Guide', 'Complete guide to modern programming', 59.99, 20, 4, 'BK-001', 4.7, '{now}')
                ");
            }
        }

        private static void SeedTasks(IDataSource ds)
        {
            var taskCount = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Tasks"));
            if (taskCount == 0)
            {
                var now = DateTime.UtcNow.ToString("o");
                var tomorrow = DateTime.UtcNow.AddDays(1).ToString("o");
                var nextWeek = DateTime.UtcNow.AddDays(7).ToString("o");

                ds.ExecuteSql($@"
                    INSERT INTO Tasks (Title, Description, Status, Priority, AssignedTo, DueDate, CreatedAt) VALUES 
                    ('Update Product Catalog', 'Review and update product information', 'In Progress', 'High', 'John Doe', '{tomorrow}', '{now}'),
                    ('Prepare Monthly Report', 'Generate sales and analytics report', 'Pending', 'Medium', 'Jane Smith', '{nextWeek}', '{now}'),
                    ('Database Maintenance', 'Perform routine database cleanup', 'Completed', 'Low', 'Admin', '{now}', '{now}'),
                    ('Customer Survey', 'Design and deploy customer satisfaction survey', 'Pending', 'Medium', 'John Doe', '{nextWeek}', '{now}'),
                    ('Security Audit', 'Conduct quarterly security review', 'In Progress', 'High', 'Admin', '{tomorrow}', '{now}')
                ");
            }
        }

        private static void SeedEvents(IDataSource ds)
        {
            var eventCount = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Events"));
            if (eventCount == 0)
            {
                var now = DateTime.UtcNow.ToString("o");
                var today = DateTime.Today.ToString("o");
                var tomorrow = DateTime.Today.AddDays(1).ToString("o");
                var nextWeek = DateTime.Today.AddDays(7).ToString("o");

                ds.ExecuteSql($@"
                    INSERT INTO Events (Title, Description, StartDate, EndDate, AllDay, Location, Attendees, Color, CreatedAt) VALUES 
                    ('Team Meeting', 'Weekly team standup meeting', '{tomorrow}', '{tomorrow}', 0, 'Conference Room A', 'John Doe,Jane Smith', '#007bff', '{now}'),
                    ('Product Launch', 'Launch event for new product line', '{nextWeek}', '{nextWeek}', 1, 'Main Auditorium', 'All Staff', '#28a745', '{now}'),
                    ('Training Session', 'Software training for new employees', '{today}', '{today}', 0, 'Training Room', 'New Hires', '#ffc107', '{now}'),
                    ('Board Meeting', 'Quarterly board meeting', '{nextWeek}', '{nextWeek}', 0, 'Executive Conference Room', 'Board Members', '#dc3545', '{now}')
                ");
            }
        }

        private static void SeedCustomers(IDataSource ds)
        {
            // Seed a few demo customers if table is empty
            var count = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Customers"));
            if (count > 0) return;

            var now = DateTime.UtcNow.ToString("o");
            ds.ExecuteSql($@"INSERT INTO Customers 
                (Name, Email, Phone, Address, City, PostalCode, Country, CompanyName, ContactPerson, TaxId, CreditLimit, PaymentTerms, Status, CustomerType, CreatedAt)
                VALUES
                ('John Doe','john@example.com','+1-555-1000','123 Market St','San Francisco','94103','USA','Acme Inc.','John Doe','TX123',10000,'Net 30','Active','Standard','{now}'),
                ('Jane Smith','jane@example.com','+1-555-2000','456 Mission St','San Francisco','94105','USA','Globex Corp.','Jane Smith','TX456',20000,'Net 30','Active','Premium','{now}'),
                ('Mike Johnson','mike@example.com','+1-555-3000','789 Howard St','San Francisco','94107','USA','Initech','Mike Johnson','TX789',5000,'Net 15','Inactive','Standard','{now}')");
        }
    }
}
