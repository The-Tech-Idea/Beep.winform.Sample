using System;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int? ID { get; internal set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
        public string Category { get; set; } = ""; // Added missing Category property
        public string? SKU { get; set; }
        public float Rating { get; set; } = 0.0f;
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Added missing CreatedDate property
        public DateTime? UpdatedAt { get; set; }
        
    
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Medium";
        public string? AssignedTo { get; set; }
        public DateTime? DueDate { get; set; }
        public int Progress { get; set; } = 0;
        public string? Tags { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AllDay { get; set; } = false;
        public string? Location { get; set; }
        public string? Attendees { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Analytics
    {
        public int Id { get; set; }
        public string MetricName { get; set; } = "";
        public double MetricValue { get; set; }
        public DateTime MetricDate { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}