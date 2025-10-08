using System;
using System.Collections.Generic;
using System.Linq;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;

namespace WinFormsApp.UI.Test.SampleBusinessApp.ViewModels
{
    public class ProductViewModel
    {
        private readonly IDMEEditor _editor;
        public ProductViewModel(IDMEEditor editor)
        {
            _editor = editor;
        }

        public List<Product> GetAll()
        {
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
            ds?.Openconnection();
            return ds?.GetData<Product>("SELECT ID, Name, Description, Price, Stock, CreatedAt, UpdatedAt FROM Products ORDER BY ID DESC") ?? new List<Product>();
        }

        // Added missing GetProducts method (alias for GetAll for consistency)
        public List<Product> GetProducts()
        {
            return GetAll();
        }

        public Product? GetById(int id)
        {
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
            ds?.Openconnection();
            // Dapper in IRDBSource supports parameters via object only in SaveData; for GetData we inline integer safely
            return ds?.GetData<Product>($"SELECT ID, Name, Description, Price, Stock, CreatedAt, UpdatedAt FROM Products WHERE ID = {id}")?.FirstOrDefault();
        }

        public ErrorsInfo Create(string name, string? description, decimal price, int stock)
        {
            var validation = Validate(name, price, stock);
            if (!string.IsNullOrEmpty(validation))
            {
                return new ErrorsInfo { Flag = Errors.Failed, Message = validation };
            }

            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
            ds?.Openconnection();

            var sql = "INSERT INTO Products (Name, Description, Price, Stock, CreatedAt) VALUES (@Name, @Description, @Price, @Stock, @CreatedAt)";
            ds?.SaveData(sql, new { Name = name, Description = description, Price = price, Stock = stock, CreatedAt = DateTime.UtcNow })?.Wait();

            return new ErrorsInfo { Flag = Errors.Ok };
        }

        // Added missing AddProduct method
        public ErrorsInfo AddProduct(Product product)
        {
            return Create(product.Name, product.Description, product.Price, product.Stock);
        }

        public ErrorsInfo Update(int id, string name, string? description, decimal price, int stock)
        {
            var validation = Validate(name, price, stock);
            if (!string.IsNullOrEmpty(validation))
            {
                return new ErrorsInfo { Flag = Errors.Failed, Message = validation };
            }

            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
            ds?.Openconnection();

            var sql = @"UPDATE Products SET Name=@Name, Description=@Description, Price=@Price, Stock=@Stock, UpdatedAt=@UpdatedAt WHERE ID=@ID";
            ds?.SaveData(sql, new { Id = id, Name = name, Description = description, Price = price, Stock = stock, UpdatedAt = DateTime.UtcNow })?.Wait();

            return new ErrorsInfo { Flag = Errors.Ok };
        }

        // Added missing UpdateProduct method
        public ErrorsInfo UpdateProduct(Product product)
        {
            var id = product.ID ?? product.Id;
            return Update(id, product.Name, product.Description, product.Price, product.Stock);
        }

        public ErrorsInfo Delete(int id)
        {
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
            ds?.Openconnection();
            var sql = "DELETE FROM Products WHERE ID=@ID";
            ds?.SaveData(sql, new { Id = id })?.Wait();
            return new ErrorsInfo { Flag = Errors.Ok };
        }

        // Added missing DeleteProduct method
        public ErrorsInfo DeleteProduct(int id)
        {
            return Delete(id);
        }

        // Added missing SearchProducts method
        public List<Product> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetProducts();
            }

            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as IRDBSource;
            ds?.Openconnection();
            
            var sql = @"SELECT ID, Name, Description, Price, Stock, CreatedAt, UpdatedAt 
                       FROM Products 
                       WHERE Name LIKE @SearchTerm OR Description LIKE @SearchTerm 
                       ORDER BY ID DESC";
            
            var searchPattern = $"%{searchTerm}%";
            return ds?.GetData<Product>(sql.Replace("@SearchTerm", $"'{searchPattern}'")) ?? new List<Product>();
        }

        // Added missing GetProductsByCategory method
        public List<Product> GetProductsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category) || category == "All Categories")
            {
                return GetProducts();
            }

            // For now, we'll simulate category filtering since the database doesn't have category column
            // In a real application, you would modify the SQL to include category filtering
            var allProducts = GetProducts();
            
            // Simulate filtering by setting categories based on product names for demo purposes
            foreach (var product in allProducts)
            {
                if (product.Name.ToLower().Contains("software") || product.Name.ToLower().Contains("license"))
                    product.Category = "Software";
                else if (product.Name.ToLower().Contains("book"))
                    product.Category = "Books";
                else if (product.Name.ToLower().Contains("headphones") || product.Name.ToLower().Contains("wireless"))
                    product.Category = "Electronics";
                else if (product.Name.ToLower().Contains("shirt") || product.Name.ToLower().Contains("clothing"))
                    product.Category = "Clothing";
                else
                    product.Category = "General";
                
                product.CreatedDate = product.CreatedAt; // Sync CreatedDate with CreatedAt
            }
            
            return allProducts.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private string? Validate(string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Name is required";
            if (name.Length > 200) return "Name too long";
            if (price < 0) return "Price must be >= 0";
            if (stock < 0) return "Stock must be >= 0";
            return null;
        }
    }
}
