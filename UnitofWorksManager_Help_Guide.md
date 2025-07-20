# UnitofWorksManager - Oracle Forms Style Data Management

## Overview

The `UnitofWorksManager` is a comprehensive data management system that simulates Oracle Forms environment with master-detail relationships, triggers, and form-level operations management. It provides dirty state checking, transaction management, and event-driven programming patterns similar to Oracle Forms.

## Table of Contents

1. [Basic Setup](#basic-setup)
2. [Block Registration](#block-registration)
3. [Master-Detail Relationships](#master-detail-relationships)
4. [Oracle Forms Style Triggers](#oracle-forms-style-triggers)
5. [Dirty State Management](#dirty-state-management)
6. [Navigation Operations](#navigation-operations)
7. [CRUD Operations](#crud-operations)
8. [Transaction Management](#transaction-management)
9. [Event Handling Examples](#event-handling-examples)
10. [Common Patterns](#common-patterns)
11. [Best Practices](#best-practices)

---

## Basic Setup

### Creating a UnitofWorksManager

```csharp
// Initialize the manager
var unitofWorksManager = new UnitofWorksManager(dmeEditor);

// Open a form
await unitofWorksManager.OpenFormAsync("CustomerOrderForm");
```

---

## Block Registration

### Registering Data Blocks

```csharp
// Create UnitOfWork instances for your entities
var customerUnitOfWork = new UnitofWork<Customer>(dmeEditor, "MyDatabase", "Customers", "CustomerID");
var orderUnitOfWork = new UnitofWork<Order>(dmeEditor, "MyDatabase", "Orders", "OrderID");

// Register blocks with the manager
unitofWorksManager.RegisterBlock(
    blockName: "CustomerBlock",
    unitOfWork: customerUnitOfWork,
    entityStructure: customerEntityStructure,
    dataSourceName: "MyDatabase",
    isMasterBlock: true
);

unitofWorksManager.RegisterBlock(
    blockName: "OrderBlock", 
    unitOfWork: orderUnitOfWork,
    entityStructure: orderEntityStructure,
    dataSourceName: "MyDatabase",
    isMasterBlock: false
);
```

---

## Master-Detail Relationships

### Creating Master-Detail Relations

```csharp
// Create a one-to-many relationship between Customer and Orders
unitofWorksManager.CreateMasterDetailRelation(
    masterBlockName: "CustomerBlock",
    detailBlockName: "OrderBlock", 
    masterKeyField: "CustomerID",
    detailForeignKeyField: "CustomerID",
    relationshipType: RelationshipType.OneToMany
);

// Create nested detail relationships (Customer -> Orders -> OrderItems)
unitofWorksManager.CreateMasterDetailRelation(
    masterBlockName: "OrderBlock",
    detailBlockName: "OrderItemBlock",
    masterKeyField: "OrderID", 
    detailForeignKeyField: "OrderID",
    relationshipType: RelationshipType.OneToMany
);
```

---

## Oracle Forms Style Triggers

### Pre-Insert Trigger Example

```csharp
// Subscribe to Pre-Insert trigger
unitofWorksManager.OnPreInsert += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Set audit fields automatically
        e.SetCurrentDateTime("CreatedDate");
        e.SetCurrentUser("CreatedBy", "CurrentUserName");
        
        // Set default values
        e.SetFieldValue("Status", "Active");
        e.SetFieldValue("CustomerType", "Standard");
        
        // Generate customer code if empty
        if (e.IsFieldNullOrEmpty("CustomerCode"))
        {
            var nextCode = GenerateNextCustomerCode();
            e.SetFieldValue("CustomerCode", nextCode);
        }
        
        // Validate required fields
        if (e.IsFieldNullOrEmpty("CompanyName"))
        {
            e.Cancel = true;
            e.Message = "Company Name is required";
            return;
        }
    }
};
```

### Pre-Update Trigger Example

```csharp
unitofWorksManager.OnPreUpdate += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Update audit fields
        e.SetCurrentDateTime("ModifiedDate");
        e.SetCurrentUser("ModifiedBy", "CurrentUserName");
        
        // Business logic validation
        var creditLimit = e.GetFieldValue("CreditLimit");
        if (creditLimit != null && (decimal)creditLimit > 100000)
        {
            // Require approval for high credit limits
            var approval = MessageBox.Show(
                "Credit limit exceeds $100,000. Do you have approval?",
                "Approval Required",
                MessageBoxButtons.YesNo);
                
            if (approval != DialogResult.Yes)
            {
                e.Cancel = true;
                e.Message = "Update cancelled - approval required for high credit limits";
                return;
            }
        }
    }
};
```

### Pre-Delete Trigger Example

```csharp
unitofWorksManager.OnPreDelete += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Check for dependent records
        var customerId = e.GetFieldValue("CustomerID");
        if (HasPendingOrders(customerId))
        {
            e.Cancel = true;
            e.Message = "Cannot delete customer with pending orders";
            return;
        }
        
        // Confirmation dialog
        var result = MessageBox.Show(
            "Are you sure you want to delete this customer?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
            
        if (result != DialogResult.Yes)
        {
            e.Cancel = true;
            return;
        }
    }
};
```

### Post-Query Trigger Example

```csharp
unitofWorksManager.OnPostQuery += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Calculate derived fields after query
        var customers = unitofWorksManager.GetUnitOfWork("CustomerBlock");
        foreach (var customer in customers.Units)
        {
            // Calculate total order amount
            var totalOrders = CalculateTotalOrders(customer.CustomerID);
            // Set calculated field (if it exists)
            SetCalculatedField(customer, "TotalOrderAmount", totalOrders);
        }
    }
};
```

---

## Dirty State Management

### Handling Unsaved Changes

```csharp
// Subscribe to unsaved changes event
unitofWorksManager.OnUnsavedChanges += (sender, e) => {
    // Show user dialog with options
    var message = $"You have unsaved changes in {e.DirtyBlocks.Count} block(s):\n" +
                  string.Join(", ", e.DirtyBlocks) + "\n\n" +
                  "What would you like to do?";
                  
    var result = MessageBox.Show(
        message,
        "Unsaved Changes",
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question);
        
    e.UserChoice = result switch {
        DialogResult.Yes => UnsavedChangesAction.Save,      // Save changes
        DialogResult.No => UnsavedChangesAction.Discard,   // Discard changes  
        _ => UnsavedChangesAction.Cancel                    // Cancel operation
    };
};

// Navigation with automatic dirty checking
private async void NextButton_Click(object sender, EventArgs e)
{
    // This will automatically check for unsaved changes before navigating
    await unitofWorksManager.NextRecordAsync("CustomerBlock");
}

// Block switching with dirty checking
private async void SwitchToOrdersButton_Click(object sender, EventArgs e)
{
    // This will check for unsaved changes before switching blocks
    await unitofWorksManager.SwitchToBlockAsync("OrderBlock");
}
```

---

## Navigation Operations

### Record Navigation Examples

```csharp
// Navigate to first record
private async void FirstButton_Click(object sender, EventArgs e)
{
    var success = await unitofWorksManager.FirstRecordAsync("CustomerBlock");
    UpdateNavigationButtons();
}

// Navigate to next record
private async void NextButton_Click(object sender, EventArgs e)
{
    var success = await unitofWorksManager.NextRecordAsync("CustomerBlock");
    UpdateNavigationButtons();
}

// Navigate to previous record  
private async void PreviousButton_Click(object sender, EventArgs e)
{
    var success = await unitofWorksManager.PreviousRecordAsync("CustomerBlock");
    UpdateNavigationButtons();
}

// Navigate to last record
private async void LastButton_Click(object sender, EventArgs e)
{
    var success = await unitofWorksManager.LastRecordAsync("CustomerBlock");
    UpdateNavigationButtons();
}

// Handle current record changes
unitofWorksManager.OnCurrentChanged += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Update UI when current record changes
        UpdateFormFields();
        RefreshCalculatedFields();
    }
};
```

---

## CRUD Operations

### Insert Operations

```csharp
// Insert new record
private async void NewButton_Click(object sender, EventArgs e)
{
    var customer = new Customer
    {
        CompanyName = companyNameTextBox.Text,
        ContactName = contactNameTextBox.Text,
        // ... other fields
    };
    
    var success = await unitofWorksManager.InsertRecordAsync("CustomerBlock", customer);
    if (success)
    {
        MessageBox.Show("Customer created successfully");
        ClearForm();
    }
}

// Insert with sequence generation
unitofWorksManager.OnPreInsert += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Generate next ID using sequence
        var blockInfo = unitofWorksManager.GetBlock("CustomerBlock");
        var nextId = unitofWorksManager.ExecuteSequence("CustomerBlock", e.CurrentRecord, "CustomerID", "CUSTOMER_SEQ");
        
        // Set audit defaults
        unitofWorksManager.SetAuditDefaults(e.CurrentRecord, "CurrentUser");
    }
};
```

### Update Operations

```csharp
// Update current record
private async void SaveButton_Click(object sender, EventArgs e)
{
    // Get current customer
    var customerUnitOfWork = unitofWorksManager.GetUnitOfWork("CustomerBlock");
    var currentCustomer = customerUnitOfWork.CurrentItem as Customer;
    
    if (currentCustomer != null)
    {
        // Update fields from form
        currentCustomer.CompanyName = companyNameTextBox.Text;
        currentCustomer.ContactName = contactNameTextBox.Text;
        // ... other fields
        
        // The UnitOfWork will automatically track the changes
        // and the manager will handle dirty state checking
    }
}
```

### Delete Operations

```csharp
// Delete current record
private async void DeleteButton_Click(object sender, EventArgs e)
{
    var success = await unitofWorksManager.DeleteCurrentRecordAsync("CustomerBlock");
    if (success)
    {
        MessageBox.Show("Customer deleted successfully");
    }
}
```

---

## Transaction Management

### Form-Level Transactions

```csharp
// Commit all changes in the form
private async void CommitButton_Click(object sender, EventArgs e)
{
    var result = await unitofWorksManager.CommitFormAsync();
    
    if (result.Flag == Errors.Ok)
    {
        MessageBox.Show("All changes saved successfully");
    }
    else
    {
        MessageBox.Show($"Error saving changes: {result.Message}");
    }
}

// Rollback all changes in the form
private async void RollbackButton_Click(object sender, EventArgs e)
{
    var result = await unitofWorksManager.RollbackFormAsync();
    
    if (result.Flag == Errors.Ok)
    {
        MessageBox.Show("All changes rolled back");
        RefreshAllBlocks();
    }
}

// Check for dirty state before closing
private async void FormClosing(object sender, FormClosingEventArgs e)
{
    if (unitofWorksManager.HasUnsavedChanges())
    {
        var result = MessageBox.Show(
            "You have unsaved changes. Do you want to save before closing?",
            "Unsaved Changes",
            MessageBoxButtons.YesNoCancel);
            
        if (result == DialogResult.Yes)
        {
            var commitResult = await unitofWorksManager.CommitFormAsync();
            if (commitResult.Flag != Errors.Ok)
            {
                e.Cancel = true; // Cancel form closing
                return;
            }
        }
        else if (result == DialogResult.Cancel)
        {
            e.Cancel = true; // Cancel form closing
            return;
        }
    }
    
    await unitofWorksManager.CloseFormAsync();
}
```

---

## Event Handling Examples

### Block-Level Events

```csharp
// Block enter event
unitofWorksManager.OnBlockEnter += (sender, e) => {
    Console.WriteLine($"Entered block: {e.BlockName}");
    SetBlockSpecificToolbar(e.BlockName);
};

// Block leave event  
unitofWorksManager.OnBlockLeave += (sender, e) => {
    Console.WriteLine($"Leaving block: {e.BlockName}");
    ValidateBlockData(e.BlockName);
};

// Block validation
unitofWorksManager.OnBlockValidate += (sender, e) => {
    if (e.BlockName == "CustomerBlock")
    {
        // Validate all customers in the block
        var isValid = ValidateAllCustomers();
        if (!isValid)
        {
            e.Cancel = true;
            e.Message = "Validation failed for customer data";
        }
    }
};
```

### Field-Level Validation

```csharp
// Field validation
unitofWorksManager.OnValidateField += (sender, e) => {
    if (e.BlockName == "CustomerBlock" && e.FieldName == "Email")
    {
        var email = e.Value?.ToString();
        if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
        {
            e.IsValid = false;
            e.ValidationMessage = "Please enter a valid email address";
        }
    }
    
    if (e.BlockName == "CustomerBlock" && e.FieldName == "CreditLimit")
    {
        if (e.Value != null && (decimal)e.Value < 0)
        {
            e.IsValid = false;
            e.ValidationMessage = "Credit limit cannot be negative";
        }
    }
};
```

### Error Handling

```csharp
// Global error handling
unitofWorksManager.OnError += (sender, e) => {
    LogError($"Error in block '{e.BlockName}': {e.ErrorMessage}", e.Exception);
    
    // Show user-friendly error message
    MessageBox.Show(
        $"An error occurred: {e.ErrorMessage}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error);
};
```

---

## Common Patterns

### Query Mode Pattern (Oracle Forms ENTER_QUERY / EXECUTE_QUERY)

```csharp
// Enter query mode
private async void QueryButton_Click(object sender, EventArgs e)
{
    var success = await unitofWorksManager.EnterQueryAsync("CustomerBlock");
    if (success)
    {
        SetFormToQueryMode();
        MessageBox.Show("Enter your search criteria and click Execute Query");
    }
}

// Execute query
private async void ExecuteQueryButton_Click(object sender, EventArgs e)
{
    // Build filters from form fields
    var filters = new List<AppFilter>();
    
    if (!string.IsNullOrEmpty(companyNameTextBox.Text))
    {
        filters.Add(new AppFilter
        {
            FieldName = "CompanyName",
            Operator = "LIKE",
            FilterValue = $"%{companyNameTextBox.Text}%"
        });
    }
    
    if (!string.IsNullOrEmpty(cityTextBox.Text))
    {
        filters.Add(new AppFilter
        {
            FieldName = "City", 
            Operator = "=",
            FilterValue = cityTextBox.Text
        });
    }
    
    var success = await unitofWorksManager.ExecuteQueryAsync("CustomerBlock", filters);
    if (success)
    {
        SetFormToBrowseMode();
        MessageBox.Show($"Query executed successfully");
    }
}

private void SetFormToQueryMode()
{
    // Clear all fields
    ClearAllFields();
    
    // Enable query fields
    companyNameTextBox.Enabled = true;
    cityTextBox.Enabled = true;
    
    // Show query buttons
    executeQueryButton.Visible = true;
    cancelQueryButton.Visible = true;
}

private void SetFormToBrowseMode()
{
    // Show navigation buttons
    firstButton.Visible = true;
    previousButton.Visible = true;
    nextButton.Visible = true;
    lastButton.Visible = true;
    
    // Hide query buttons
    executeQueryButton.Visible = false;
    cancelQueryButton.Visible = false;
}
```

### Calculated Fields Pattern

```csharp
// Update calculated fields when detail records change
unitofWorksManager.OnPostInsert += (sender, e) => {
    if (e.BlockName == "OrderItemBlock")
    {
        UpdateOrderTotals();
    }
};

unitofWorksManager.OnPostUpdate += (sender, e) => {
    if (e.BlockName == "OrderItemBlock")
    {
        UpdateOrderTotals();
    }
};

unitofWorksManager.OnPostDelete += (sender, e) => {
    if (e.BlockName == "OrderItemBlock")
    {
        UpdateOrderTotals();
    }
};

private void UpdateOrderTotals()
{
    var orderUnitOfWork = unitofWorksManager.GetUnitOfWork("OrderBlock");
    var orderItemUnitOfWork = unitofWorksManager.GetUnitOfWork("OrderItemBlock");
    
    if (orderUnitOfWork.CurrentItem is Order currentOrder)
    {
        var total = orderItemUnitOfWork.Units
            .Cast<OrderItem>()
            .Where(item => item.OrderID == currentOrder.OrderID)
            .Sum(item => item.Quantity * item.UnitPrice);
            
        currentOrder.TotalAmount = total;
        
        // Update display
        totalAmountLabel.Text = total.ToString("C");
    }
}
```

### Audit Trail Pattern

```csharp
// Comprehensive audit trail implementation
unitofWorksManager.OnPreInsert += (sender, e) => {
    SetAuditFieldsForInsert(e);
};

unitofWorksManager.OnPreUpdate += (sender, e) => {
    SetAuditFieldsForUpdate(e);
};

private void SetAuditFieldsForInsert(DMLTriggerEventArgs e)
{
    var currentUser = GetCurrentUser();
    var now = DateTime.Now;
    
    e.SetFieldValue("CreatedBy", currentUser);
    e.SetFieldValue("CreatedDate", now);
    e.SetFieldValue("ModifiedBy", currentUser);
    e.SetFieldValue("ModifiedDate", now);
    e.SetFieldValue("Version", 1);
}

private void SetAuditFieldsForUpdate(DMLTriggerEventArgs e)
{
    var currentUser = GetCurrentUser();
    var now = DateTime.Now;
    
    e.SetFieldValue("ModifiedBy", currentUser);
    e.SetFieldValue("ModifiedDate", now);
    
    // Increment version
    var currentVersion = e.GetFieldValue("Version");
    if (currentVersion != null)
    {
        e.SetFieldValue("Version", (int)currentVersion + 1);
    }
}
```

---

## Best Practices

### 1. Proper Resource Management

```csharp
public class CustomerOrderForm : Form, IDisposable
{
    private UnitofWorksManager unitofWorksManager;
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            unitofWorksManager?.Dispose();
        }
        base.Dispose(disposing);
    }
}
```

### 2. Centralized Configuration

```csharp
public class FormManager
{
    public static void ConfigureStandardTriggers(UnitofWorksManager manager)
    {
        // Standard audit field handling
        manager.OnPreInsert += SetAuditFieldsOnInsert;
        manager.OnPreUpdate += SetAuditFieldsOnUpdate;
        
        // Standard validation
        manager.OnValidateField += ValidateCommonFields;
        
        // Standard error handling
        manager.OnError += LogAndDisplayErrors;
    }
}
```

### 3. Type-Safe Field Access

```csharp
public static class CustomerFields
{
    public const string CustomerID = "CustomerID";
    public const string CompanyName = "CompanyName";
    public const string ContactName = "ContactName";
    public const string Email = "Email";
    public const string CreditLimit = "CreditLimit";
}

// Usage in triggers
unitofWorksManager.OnValidateField += (sender, e) => {
    if (e.FieldName == CustomerFields.Email)
    {
        // Validate email
    }
};
```

### 4. Async/Await Best Practices

```csharp
// Always use async/await for database operations
private async void SaveButton_Click(object sender, EventArgs e)
{
    try
    {
        saveButton.Enabled = false;
        loadingIndicator.Visible = true;
        
        var result = await unitofWorksManager.CommitFormAsync();
        
        if (result.Flag == Errors.Ok)
        {
            ShowSuccessMessage("Changes saved successfully");
        }
        else
        {
            ShowErrorMessage($"Error saving: {result.Message}");
        }
    }
    catch (Exception ex)
    {
        ShowErrorMessage($"Unexpected error: {ex.Message}");
    }
    finally
    {
        saveButton.Enabled = true;
        loadingIndicator.Visible = false;
    }
}
```

### 5. Performance Considerations

```csharp
// Use filtering to limit data retrieval
private async void LoadCustomers()
{
    var filters = new List<AppFilter>
    {
        new AppFilter { FieldName = "Status", Operator = "=", FilterValue = "Active" },
        new AppFilter { FieldName = "CreatedDate", Operator = ">=", FilterValue = DateTime.Now.AddYears(-1) }
    };
    
    await unitofWorksManager.ExecuteQueryAsync("CustomerBlock", filters);
}

// Implement paging for large datasets
private async void LoadPage(int pageNumber, int pageSize)
{
    var filters = new List<AppFilter>
    {
        new AppFilter { FieldName = "PageIndex", Operator = "=", FilterValue = pageNumber.ToString() },
        new AppFilter { FieldName = "PageSize", Operator = "=", FilterValue = pageSize.ToString() }
    };
    
    await unitofWorksManager.ExecuteQueryAsync("CustomerBlock", filters);
}
```

---

## Conclusion

The UnitofWorksManager provides a powerful, Oracle Forms-like environment for managing complex data operations with automatic dirty state checking, comprehensive trigger support, and robust transaction management. By following these patterns and examples, you can build sophisticated data entry applications with minimal code while maintaining data integrity and providing excellent user experience.

For more advanced scenarios, refer to the API documentation and consider extending the manager with custom triggers and validation logic specific to your business requirements.