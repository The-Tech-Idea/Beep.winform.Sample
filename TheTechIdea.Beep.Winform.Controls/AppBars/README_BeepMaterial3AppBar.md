# BeepMaterial3AppBar - Material Design 3 Top App Bar

A Material Design 3 compliant top app bar component built on the BaseControl foundation, following the official Material 3 specifications from https://m3.material.io/components/app-bars/specs.

## Features

- **Material 3 Compliant**: Follows official MD3 specifications for sizing, spacing, and behavior
- **Multiple Variants**: Small, Center-aligned, Medium, and Large variants
- **Navigation Icon**: Optional navigation icon (typically menu or back button)
- **Title Display**: Left-aligned or center-aligned title text
- **Search Integration**: Built-in search field using BeepTextBox
- **Action Buttons**: Up to 3 customizable action buttons
- **Overflow Menu**: Dropdown menu for additional actions using BeepPopupListForm
- **Theme Support**: Full integration with Beep theme system
- **DPI Aware**: All measurements scale properly with system DPI

## Material 3 Specifications

- **Small/Center-aligned**: 64dp height
- **Medium**: 112dp height  
- **Large**: 152dp height
- **Touch Targets**: 48dp minimum touch area
- **Icon Size**: 24dp graphic size
- **Padding**: 16dp start/end padding
- **Spacing**: 16dp between elements

## Basic Usage

### Simple Top App Bar

```csharp
var appBar = new BeepMaterial3AppBar
{
    Dock = DockStyle.Top,
    Title = "My Application",
    NavigationIconPath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.menu.svg",
    Variant = Material3AppBarVariant.Small
};

// Handle navigation click
appBar.NavigationClicked += (s, e) => ToggleDrawer();

this.Controls.Add(appBar);
```

### With Action Buttons

```csharp
var appBar = new BeepMaterial3AppBar
{
    Dock = DockStyle.Top,
    Title = "Dashboard",
    Variant = Material3AppBarVariant.Small
};

// Set up action buttons
appBar.SetActionButton(1, "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.079-search.svg");
appBar.SetActionButton(2, "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.093-waving.svg");
appBar.SetActionButton(3, "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.025-user.svg");

// Handle action button clicks
appBar.ActionButtonClicked += (s, e) =>
{
    switch (e.ButtonIndex)
    {
        case 1: appBar.ActivateSearch(); break;
        case 2: ShowNotifications(); break;
        case 3: ShowUserProfile(); break;
    }
};
```

### With Search and Overflow Menu

```csharp
var appBar = new BeepMaterial3AppBar
{
    Dock = DockStyle.Top,
    Title = "Enterprise App",
    ShowSearch = true,
    Variant = Material3AppBarVariant.Small
};

// Add overflow menu items
appBar.AddOverflowMenuItem("Settings", "settings-icon.svg");
appBar.AddOverflowMenuItem("Help", "help-icon.svg");
appBar.AddOverflowMenuItem("About", "info-icon.svg");

// Handle search
appBar.SearchChanged += (s, e) => PerformSearch(e.SearchText);

// Handle overflow menu selection
appBar.OverflowItemSelected += (s, e) =>
{
    switch (e.SelectedItem.Text)
    {
        case "Settings": OpenSettings(); break;
        case "Help": ShowHelp(); break;
        case "About": ShowAbout(); break;
    }
};
```

### Center-Aligned Variant

```csharp
var appBar = new BeepMaterial3AppBar
{
    Dock = DockStyle.Top,
    Title = "Centered Title",
    Variant = Material3AppBarVariant.CenterAligned,
    NavigationIconPath = "back-arrow.svg"
};

appBar.NavigationClicked += (s, e) => this.Close();
```

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Variant` | Material3AppBarVariant | Small | App bar variant (Small, CenterAligned, Medium, Large) |
| `Title` | string | "App Title" | Title text displayed in the app bar |
| `ShowSearch` | bool | false | Show or hide the search field |
| `NavigationIconPath` | string | "" | SVG path for navigation icon |
| `ShowDivider` | bool | false | Show bottom divider line |
| `OverflowMenuItems` | BindingList<SimpleItem> | Empty | Items in the overflow menu |

## Events

| Event | Description |
|-------|-------------|
| `NavigationClicked` | Fired when navigation icon is clicked |
| `ActionButtonClicked` | Fired when action button is clicked (provides button index) |
| `SearchChanged` | Fired when search text changes |
| `OverflowItemSelected` | Fired when overflow menu item is selected |

## Methods

| Method | Description |
|--------|-------------|
| `SetActionButton(index, iconPath, visible)` | Configure action button icon and visibility |
| `ActivateSearch()` | Show and focus the search field |
| `DeactivateSearch()` | Hide search field and clear text |
| `AddOverflowMenuItem(text, iconPath)` | Add item to overflow menu |
| `ShowOverflowMenu()` | Display the overflow dropdown menu |

## Theme Integration

The component automatically applies app bar-specific theme colors:

- `AppBarBackColor` - Background color
- `AppBarForeColor` - Text color  
- `AppBarTitleForeColor` - Title text color
- `AppBarButtonBackColor` - Action button background
- `AppBarButtonForeColor` - Action button foreground
- `AppBarTextBoxBackColor` - Search field background
- `AppBarTextBoxForeColor` - Search field text

## Complete Example

```csharp
public partial class MainForm : Form
{
    private BeepMaterial3AppBar appBar;

    public MainForm()
    {
        InitializeComponent();
        SetupAppBar();
    }

    private void SetupAppBar()
    {
        appBar = new BeepMaterial3AppBar
        {
            Dock = DockStyle.Top,
            Title = "Material 3 Demo",
            NavigationIconPath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.menu.svg",
            Variant = Material3AppBarVariant.Small,
            ShowDivider = true
        };

        // Configure action buttons
        appBar.SetActionButton(1, "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.079-search.svg");
        appBar.SetActionButton(2, "TheTechIdea.Beep.Winform.Controls.GFX.SVG.NAV.093-waving.svg");
        
        // Add overflow menu
        appBar.AddOverflowMenuItem("Settings");
        appBar.AddOverflowMenuItem("Help");
        appBar.AddOverflowMenuItem("About");

        // Wire up events
        appBar.NavigationClicked += (s, e) => MessageBox.Show("Navigation clicked");
        appBar.ActionButtonClicked += AppBar_ActionButtonClicked;
        appBar.SearchChanged += (s, e) => Console.WriteLine($"Search: {e.SearchText}");
        appBar.OverflowItemSelected += (s, e) => MessageBox.Show($"Selected: {e.SelectedItem.Text}");

        this.Controls.Add(appBar);
    }

    private void AppBar_ActionButtonClicked(object sender, BeepMaterial3AppBar.ActionButtonClickedEventArgs e)
    {
        switch (e.ButtonIndex)
        {
            case 1:
                appBar.ActivateSearch();
                break;
            case 2:
                MessageBox.Show("Notifications");
                break;
        }
    }
}
```

This Material 3 AppBar provides a clean, modern interface that follows Google's Material Design 3 specifications while integrating seamlessly with the Beep control system.