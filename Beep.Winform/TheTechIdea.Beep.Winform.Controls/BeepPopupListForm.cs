public void InitializeMenu(List<SimpleItem> items)
{
    _beepListBox.TextFont = _textFont;
    _beepListBox.ListItems = new BindingList<SimpleItem>(items);
    _beepListBox.Theme = Theme;
    _beepListBox.ApplyThemeOnImage = false;
    _beepListBox.IsRoundedAffectedByTheme = false;
    _beepListBox.IsRounded = false;
    _beepListBox.ShowTitle = false;
    _beepListBox.ShowTitleLine = false;
    _beepListBox.IsShadowAffectedByTheme = false;
    _beepListBox.ShowShadow = false;
    _beepListBox.IsBorderAffectedByTheme = false;
    _beepListBox.ShowAllBorders = false;
    _beepListBox.IsFrameless = true;

    _beepListBox.ShowHilightBox = false;
    _beepListBox.MenuItemHeight = Math.Max(Menuitemheight, 20); // Ensure minimum height

    // Get the actual needed height from BeepListBox
    int neededHeight = _beepListBox.GetMaxHeight();

    // Calculate max width with proper scaling
    int calculatedMaxWidth = 150; // Minimum width
    foreach (var item in items)
    {
        if (!string.IsNullOrEmpty(item.Text))
        {
            int textWidth = TextRenderer.MeasureText(item.Text, _beepListBox.TextFont).Width;
            calculatedMaxWidth = Math.Max(calculatedMaxWidth, textWidth + 40); // Add padding
        }
    }

    // Ensure reasonable bounds - allow much larger height or no limit
    calculatedMaxWidth = Math.Min(calculatedMaxWidth, 400); // Max width
    // Remove the height cap to allow all items to be displayed
    // neededHeight = Math.Min(neededHeight, 300); // <- REMOVED: This was preventing all items from showing
    neededHeight = neededHeight;// Math.Max(neededHeight, 60);  // Keep minimum height

    // Ensure we have valid dimensions before setting size
    calculatedMaxWidth = Math.Max(calculatedMaxWidth, 10); // Minimum width
    neededHeight = Math.Max(neededHeight, 10); // Minimum height

    // Set the form size
    Size = new Size(calculatedMaxWidth, neededHeight);
    _beepListBox.Dock = DockStyle.Fill;
    _beepListBox.Invalidate();
}