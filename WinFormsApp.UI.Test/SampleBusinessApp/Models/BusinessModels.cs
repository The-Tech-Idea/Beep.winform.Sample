using System;
using System.ComponentModel.DataAnnotations;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Models
{
    /// <summary>
    /// Customer entity for CRM and sales management
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(50)]
        public string Country { get; set; } = string.Empty;

        // Business fields
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [StringLength(50)]
        public string TaxId { get; set; } = string.Empty;

        public decimal CreditLimit { get; set; } = 0;

        [StringLength(20)]
        public string PaymentTerms { get; set; } = "Net 30";

        // Status and tracking
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive, Suspended

        [StringLength(20)]
        public string CustomerType { get; set; } = "Standard"; // Standard, Premium, VIP

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastContactDate { get; set; }

        // Computed properties
        public string DisplayName => !string.IsNullOrEmpty(CompanyName) ? CompanyName : Name;
        public string FullAddress => $"{Address}, {City}, {PostalCode}, {Country}".Trim(' ', ',');
    }

    /// <summary>
    /// Sales Order entity
    /// </summary>
    public class SalesOrder
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Shipped, Delivered, Cancelled

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TotalAmount { get; set; }

        [StringLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        [StringLength(50)]
        public string SalesRep { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public List<OrderLineItem> LineItems { get; set; } = new List<OrderLineItem>();
    }

    /// <summary>
    /// Order line item entity
    /// </summary>
    public class OrderLineItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public SalesOrder Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal LineTotal { get; set; }

        [StringLength(200)]
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Invoice entity
    /// </summary>
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;

        public int? OrderId { get; set; }
        public SalesOrder Order { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public decimal BalanceAmount { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue, Cancelled

        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Partial, Paid

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }

    /// <summary>
    /// Payment entity
    /// </summary>
    public class Payment
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Check, Credit Card, Bank Transfer

        [StringLength(100)]
        public string Reference { get; set; } = string.Empty; // Check number, transaction ID, etc.

        [StringLength(20)]
        public string Status { get; set; } = "Completed"; // Pending, Completed, Failed, Cancelled

        [StringLength(200)]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Enhanced Category entity
    /// </summary>
    public class ProductCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public ProductCategory Parent { get; set; }

        [StringLength(20)]
        public string Color { get; set; } = "#007bff";

        [StringLength(50)]
        public string Icon { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public List<ProductCategory> Children { get; set; } = new List<ProductCategory>();
        public List<Product> Products { get; set; } = new List<Product>();
    }

    /// <summary>
    /// Enhanced User entity
    /// </summary>
    public class BusinessUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20)]
        public string Role { get; set; } = "User"; // Admin, Manager, User, ReadOnly

        [StringLength(500)]
        public string Permissions { get; set; } = string.Empty; // JSON string of permissions

        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Inactive, Locked

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Security fields
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockedUntil { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }
    }

    /// <summary>
    /// Audit log entity for tracking changes
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EntityName { get; set; } = string.Empty;

        public int EntityId { get; set; }

        [Required]
        [StringLength(20)]
        public string Action { get; set; } = string.Empty; // Create, Update, Delete, View

        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(2000)]
        public string Changes { get; set; } = string.Empty; // JSON string of changes

        [StringLength(200)]
        public string UserAgent { get; set; } = string.Empty;

        [StringLength(50)]
        public string IpAddress { get; set; } = string.Empty;
    }
}