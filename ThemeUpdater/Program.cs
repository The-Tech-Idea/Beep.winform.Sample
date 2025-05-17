// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Main();
static void Main()
{
    string themeFolderPath = @"..\Beep.Winform\TheTechIdea.Beep.Vis.Modules2.0\ThemeTypes\NeumorphismTheme\Parts";
    string themeFilePath = Path.Combine(themeFolderPath, "theme.txt");

    // Step 1: Extract properties and values from theme.txt
    var properties = ExtractProperties(themeFilePath);

    // Step 2: Identify other theme files in the same folder
    var themeFiles = Directory.GetFiles(themeFolderPath, "*.txt");
    foreach (var file in themeFiles)
    {
        if (file.Equals(themeFilePath, StringComparison.OrdinalIgnoreCase))
            continue;

        // Step 3: Update properties in other theme files
        UpdateThemeFile(file, properties);
    }

    Console.WriteLine("Theme files updated successfully.");
}

static Dictionary<string, string> ExtractProperties(string filePath)
{
    var properties = new Dictionary<string, string>();
    var lines = File.ReadAllLines(filePath);

    var propertyRegex = new Regex(@"(\w+)\s*=\s*(.+);");

    foreach (var line in lines)
    {
        var match = propertyRegex.Match(line);
        if (match.Success)
        {
            string propertyName = match.Groups[1].Value.Trim();
            string propertyValue = match.Groups[2].Value.Trim();
            properties[propertyName] = propertyValue;
        }
    }

    return properties;
}

static void UpdateThemeFile(string filePath, Dictionary<string, string> properties)
{
    var lines = File.ReadAllLines(filePath);
    var updatedLines = new List<string>();

    var propertyRegex = new Regex(@"(\w+)\s*=\s*(.+);");

    foreach (var line in lines)
    {
        var match = propertyRegex.Match(line);
        if (match.Success)
        {
            string propertyName = match.Groups[1].Value.Trim();
            if (properties.ContainsKey(propertyName))
            {
                string updatedLine = $"{propertyName} = {properties[propertyName]};";
                updatedLines.Add(updatedLine);
                continue;
            }
        }

        updatedLines.Add(line);
    }

    File.WriteAllLines(filePath, updatedLines);
}