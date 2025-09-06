# BeepMultiChipGroup - Material Design 3 Chip Styles Plan

## ?? **Current Understanding**

After reviewing Material Design 3 specs and the provided images, I now understand:
- **ChipStyle** should define **design patterns**, NOT colors
- Colors are always handled by theme system
- Need to leverage BaseControl's `LeadingIcon` and `TrailingIcon` capabilities
- Need different structural layouts like Material Design 3

## ?? **Material Design 3 Chip Types**

Based on MD3 specs and images:

### 1. **Filter Chips**
- ? Checkmark icon (leading) when selected
- ?? Close button (trailing) optional
- Used for filtering content
- Example: "Travel ?" or "Sports ? ?"

### 2. **Input Chips** 
- ?? Avatar/icon (leading) 
- ? Close button (trailing)
- Represent user input (tags, contacts)
- Example: "?? John Doe ?"

### 3. **Assist Chips**
- ?? Action icon (leading)
- No trailing icon typically
- Help users take action
- Example: "?? Set location" or "?? Calendar"

### 4. **Suggestion Chips**
- ?? Optional icon (leading)
- No trailing icon
- Simple suggestions
- Example: "?? Music" or just "Books"

## ??? **Implementation Plan**

### **Phase 1: Redesign ChipStyle Enum** ?
Replace color-based styles with structural MD3 patterns:

```csharp
public enum ChipStyle
{
    /// <summary>Simple text-only chips</summary>
    Suggestion,
    /// <summary>Chips with leading icons for actions</summary>
    Assist,
    /// <summary>Chips with checkmarks when selected, optional close</summary>
    Filter,
    /// <summary>Chips with avatars and close buttons</summary>
    Input,
    /// <summary>Custom icon configuration</summary>
    Custom
}
```

### **Phase 2: Icon Integration** ?
Leverage BaseControl's icon system:
- Use `LeadingIcon` for checkmarks, avatars, action icons
- Use `TrailingIcon` for close buttons
- Add `ShowCloseButton` property
- Add `ShowCheckmarkWhenSelected` property

### **Phase 3: Chip Structure Enhancement** ?
Update ChipItem to include:
- Icon paths and sizes
- Close button behavior
- Avatar support
- Material spacing

### **Phase 4: Layout Updates** ?
- Material Design spacing (8dp grid)
- Proper icon positioning
- Text + icon alignment
- Close button hit area

## ?? **New Property Structure**

### **Main Style Control:**
```csharp
[Category("Chip Style")]
public ChipStyle ChipStyle { get; set; } = ChipStyle.Suggestion;
```

### **Icon Properties:**
```csharp
[Category("Chip Style")]
public bool ShowCloseButton { get; set; } = false;

[Category("Chip Style")]  
public bool ShowCheckmarkWhenSelected { get; set; } = true;

[Category("Chip Style")]
public string DefaultLeadingIcon { get; set; } = "";

[Category("Chip Style")]
public Size ChipIconSize { get; set; } = new Size(18, 18);
```

### **Enhanced ChipItem:**
```csharp
private class ChipItem
{
    public SimpleItem Item { get; set; }
    public Rectangle Bounds { get; set; }
    public Rectangle CloseButtonBounds { get; set; }
    public bool IsSelected { get; set; }
    public bool IsHovered { get; set; }
    public bool IsCloseable { get; set; }
    public string LeadingIcon { get; set; }
    public string TrailingIcon { get; set; }
    // Structure properties, NOT colors
}
```

## ?? **Style Implementations**

### **Suggestion Chips** (Simple)
- Just text, optional leading icon
- Clean, minimal appearance
- Example: "Music", "?? Books"

### **Assist Chips** (Action)
- Leading icon for action type
- No trailing elements
- Example: "?? Add location", "?? Call"

### **Filter Chips** (Selection)
- Checkmark icon when selected
- Optional close button
- Example: "? Travel", "Sports ? ?"

### **Input Chips** (Data Entry)
- Avatar/icon representing input
- Close button for removal
- Example: "?? John ?", "?? email@example.com ?"

## ??? **Implementation Steps**

### **Step 1**: Redesign ChipStyle enum ?
### **Step 2**: Add icon properties ?  
### **Step 3**: Update ChipItem structure ?
### **Step 4**: Implement style-specific layouts ?
### **Step 5**: Add close button functionality ?
### **Step 6**: Integrate with BaseControl icon system ?
### **Step 7**: Update drawing methods ?
### **Step 8**: Add proper Material spacing ?

## ?? **Key Insights from MD3 Specs**

1. **Spacing**: 8dp grid system
2. **Icons**: 18dp standard size for chip icons
3. **Height**: 32dp standard, 24dp dense
4. **Padding**: 8dp horizontal for text
5. **Border Radius**: 16dp for standard height
6. **Close button**: 18dp touch target
7. **Checkmark**: Leading position when selected

## ?? **Expected Developer Experience**

```csharp
// Filter chips for categories
var filterChips = new BeepMultiChipGroup 
{
    ChipStyle = ChipStyle.Filter,  // ? shows checkmarks, optional close
    SelectionMode = ChipSelectionMode.Multiple
};

// Input chips for tags
var inputChips = new BeepMultiChipGroup
{
    ChipStyle = ChipStyle.Input,   // ?? shows avatars, ? close buttons
    ShowCloseButton = true
};

// Action chips
var assistChips = new BeepMultiChipGroup
{
    ChipStyle = ChipStyle.Assist,  // ?? action icons
    DefaultLeadingIcon = "location"
};
```

---

This properly aligns with Material Design 3 specifications and provides the structural chip types seen in the images! ??