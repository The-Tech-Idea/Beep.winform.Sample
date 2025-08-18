# ASP.NET-Style Configuration for WinForms Sample Business App

This configuration system provides ASP.NET-style `appsettings.json` functionality for the WinForms Sample Business App, supporting multiple environments (Development, Testing, Production).

## Features

### ? **Environment Support**
- **Development** - Relaxed security, debugging features enabled
- **Testing** - Moderate security, logging enabled  
- **Production** - Strict security, minimal logging

### ? **Configuration Sources** 
- Environment-specific JSON files (`appsettings.Development.json`)
- Base configuration file (`appsettings.json`)
- Command-line arguments (`--environment Production`)
- Environment variables (`BEEP_ENVIRONMENT=Testing`)

### ? **Hierarchical Configuration**
Similar to ASP.NET Core, you can access nested settings:
```csharp
// Get database connection string
var connectionString = config.GetValue<string>("Database:ConnectionString");

// Get authentication timeout
var timeout = config.GetValue<int>("Authentication:SessionTimeoutMinutes", 60);

// Check if feature is enabled
var isEnabled = config.IsFeatureEnabled("EnableAdvancedReporting");
```

## Usage Examples

### **1. Running in Different Environments**

```bash
# Development (default)
SampleBusinessApp.exe

# Testing environment
SampleBusinessApp.exe --environment Testing

# Production environment  
SampleBusinessApp.exe -e Production
```

### **2. Configuration in Code**

```csharp
// Get configuration instance
var config = UserSettingsManager.Configuration;

// Access settings
var dbConnection = config.Settings.Database.ConnectionString;
var appName = config.Settings.ApplicationName;
var theme = config.Settings.UI.DefaultTheme;

// Environment-specific behavior
if (config.Environment == "Development") 
{
    // Enable debug features
}

// Feature flags
if (config.IsFeatureEnabled("EnableAdvancedReporting")) 
{
    // Show reporting menu
}

// User preferences
UserSettingsManager.SaveUserPreference("LastSelectedTab", "Dashboard");
var lastTab = UserSettingsManager.GetUserPreference<string>("LastSelectedTab");
```

### **3. Credential Management**

```csharp
// Save remembered username (secure, environment-specific)
UserSettingsManager.SaveRememberedUsername("john.doe");

// Get remembered username
var username = UserSettingsManager.GetRememberedUsername();

// Clear credentials
UserSettingsManager.ClearRememberedUsername();
```

### **4. Window State Management**

```csharp
// Save window bounds
UserSettingsManager.SaveWindowBounds("MainForm", this.Bounds);

// Restore window bounds
var bounds = UserSettingsManager.GetWindowBounds("MainForm");
if (bounds.HasValue) 
{
    this.Bounds = bounds.Value;
}
```

## Configuration Schema

### **Database Settings**
```json
{
  "database": {
    "connectionString": "Data Source=app.db;Version=3;",
    "provider": "SQLite",
    "commandTimeout": 30,
    "enableLogging": true,
    "maxRetryAttempts": 3,
    "retryDelaySeconds": 1
  }
}
```

### **Authentication Settings**
```json
{
  "authentication": {
    "rememberUserCredentials": false,
    "sessionTimeoutMinutes": 60,
    "maxLoginAttempts": 3,
    "accountLockoutMinutes": 15,
    "requirePasswordComplexity": true,
    "minPasswordLength": 6,
    "enableTwoFactorAuth": false,
    "defaultUsername": "",
    "autoLoginInDevelopment": false
  }
}
```

### **Feature Flags**
```json
{
  "features": {
    "enableAdvancedReporting": true,
    "enableDataExport": true,
    "enableDataImport": true,
    "enableUserManagement": false,
    "enableAuditTrail": true,
    "enableDashboardWidgets": true,
    "enableNotifications": true,
    "enableAutoSave": true,
    "enableOfflineMode": false,
    "enableDarkMode": true
  }
}
```

### **UI Settings**
```json
{
  "ui": {
    "defaultTheme": "DefaultTheme",
    "rememberWindowSize": true,
    "rememberWindowPosition": true,
    "enableAnimations": true,
    "showToolTips": true,
    "autoSaveIntervalMinutes": 5,
    "enableStatusBar": true,
    "enableToolbar": true,
    "dateFormat": "yyyy-MM-dd",
    "timeFormat": "HH:mm:ss",
    "currencyFormat": "C2"
  }
}
```

## Environment-Specific Behaviors

### **Development Environment**
- Auto-login with default credentials for testing
- Extended error messages with stack traces
- All features enabled
- Debug logging enabled
- Relaxed password requirements
- Extended timeouts

### **Testing Environment**  
- Moderate security settings
- Limited features for focused testing
- Detailed logging for troubleshooting
- Standard timeouts

### **Production Environment**
- Strict security settings
- Minimal logging (warnings/errors only)
- Strong password requirements
- Short timeouts
- Audit trail enabled
- Secure connection required

## File Locations

- **Configuration Files**: `./Configuration/appsettings.{Environment}.json`
- **User Settings**: `%AppData%/SampleBusinessApp/user_settings_{Environment}.json`
- **Log Files**: `./Logs/` (configurable)

## Benefits

1. **Environment Separation** - Different settings per environment
2. **Secure Credential Storage** - No hardcoded passwords
3. **Feature Toggles** - Enable/disable features without code changes  
4. **User Preferences** - Remember user-specific settings
5. **ASP.NET Familiarity** - Similar to ASP.NET Core configuration
6. **Type Safety** - Strongly-typed configuration classes
7. **Validation** - Built-in configuration validation
8. **Hot Reload** - Configuration changes without restart (future enhancement)

This system replaces the need for `Properties.Settings.Default` and provides a modern, flexible configuration approach for WinForms applications.

# Configuration System Migration

The configuration managers have been moved to a more appropriate location:

## New Location
**`TheTechIdea.Beep.Desktop.Common.Util.Configuration`**

This shared utility library is the proper place for configuration management functionality that can be reused across multiple Beep applications.

## Files Moved
- `AppSettings.cs` ? `../Beep.Desktop/TheTechIdea.Beep.Desktop.Common.Util/Configuration/AppSettings.cs`
- `AppConfigurationManager.cs` ? `../Beep.Desktop/TheTechIdea.Beep.Desktop.Common.Util/Configuration/AppConfigurationManager.cs`
- `UserSettingsManager.cs` ? `../Beep.Desktop/TheTechIdea.Beep.Desktop.Common.Util/Configuration/UserSettingsManager.cs`

## Updated Usage
```csharp
// Old namespace
using WinFormsApp.UI.Test.SampleBusinessApp.Configuration;

// New namespace
using TheTechIdea.Beep.Desktop.Common.Util.Configuration;

// Usage remains the same
var config = UserSettingsManager.Configuration;
```

## Benefits of the Move
1. **Reusability** - Can be used by other Beep applications
2. **Shared Utility** - Fits the purpose of the Desktop.Common.Util project
3. **Better Organization** - Configuration logic is centralized
4. **Consistency** - Follows Beep framework patterns

## Configuration Files
Environment-specific configuration files remain in the application's Configuration folder for now:
- `appsettings.Development.json`
- `appsettings.Production.json`
- `README.md`

These files are templates and will be automatically created by the configuration manager in the application's directory when first run.