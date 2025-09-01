using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;

/// <summary>
/// Test to demonstrate BeepLabel working with the shared Material Design architecture
/// </summary>
public class BeepLabelMaterialTest
{
    public static void TestBeepLabelMaterialSizing()
    {
        Console.WriteLine("=== Testing BeepLabel Material Design Architecture ===");
        
        // Test 1: Standard Material Design sizing
        var labelStandard = new BeepLabel();
        labelStandard.Text = "Material Design Label";
        labelStandard.EnableMaterialStyle = true;
        labelStandard.MaterialPreserveContentArea = false; // Standard Material sizing
        labelStandard.LabelAutoSizeForMaterial = true;
        
        Console.WriteLine($"Test 1 - Standard Material Sizing:");
        Console.WriteLine($"Initial size: {labelStandard.Width}x{labelStandard.Height}");
        Console.WriteLine(labelStandard.GetMaterialSizeInfo());
        
        // Force size compensation
        labelStandard.ForceMaterialSizeCompensation();
        Console.WriteLine($"After compensation: {labelStandard.Width}x{labelStandard.Height}");
        
        Console.WriteLine();
        
        // Test 2: Content-preserving Material Design sizing
        var labelPreserved = new BeepLabel();
        labelPreserved.Text = "Content-Preserving Label";
        labelPreserved.EnableMaterialStyle = true;
        labelPreserved.MaterialPreserveContentArea = true; // Content-preserving sizing
        labelPreserved.LabelAutoSizeForMaterial = true;
        
        Console.WriteLine($"Test 2 - Content-Preserving Material Sizing:");
        Console.WriteLine($"Initial size: {labelPreserved.Width}x{labelPreserved.Height}");
        Console.WriteLine(labelPreserved.GetMaterialSizeInfo());
        
        // Force size compensation
        labelPreserved.ForceMaterialSizeCompensation();
        Console.WriteLine($"After compensation: {labelPreserved.Width}x{labelPreserved.Height}");
        
        Console.WriteLine();
        
        // Test 3: Label with SubHeader support
        var labelWithSubheader = new BeepLabel();
        labelWithSubheader.Text = "Main Header Text";
        labelWithSubheader.SubHeaderText = "Subheader text content";
        labelWithSubheader.EnableMaterialStyle = true;
        labelWithSubheader.LabelAutoSizeForMaterial = true;
        
        Console.WriteLine($"Test 3 - Label with SubHeader:");
        Console.WriteLine($"Main text: {labelWithSubheader.Text}");
        Console.WriteLine($"Sub text: {labelWithSubheader.SubHeaderText}");
        Console.WriteLine($"Initial size: {labelWithSubheader.Width}x{labelWithSubheader.Height}");
        
        // Force size compensation
        labelWithSubheader.ForceMaterialSizeCompensation();
        Console.WriteLine($"After compensation: {labelWithSubheader.Width}x{labelWithSubheader.Height}");
        Console.WriteLine(labelWithSubheader.GetMaterialSizeInfo());
        
        Console.WriteLine();
        
        // Test 4: Material property changes trigger automatic compensation
        var labelAuto = new BeepLabel();
        labelAuto.EnableMaterialStyle = true;
        labelAuto.MaterialAutoSizeCompensation = true;
        labelAuto.Text = "Auto-sizing Label";
        
        Console.WriteLine($"Test 4 - Automatic Property Change Compensation:");
        Console.WriteLine($"Before variant change: {labelAuto.Width}x{labelAuto.Height}");
        
        // Change variant - should trigger automatic compensation
        labelAuto.MaterialVariant = TheTechIdea.Beep.Winform.Controls.Models.MaterialTextFieldVariant.Filled;
        Console.WriteLine($"After changing to Filled variant: {labelAuto.Width}x{labelAuto.Height}");
        
        // Cleanup
        labelStandard.Dispose();
        labelPreserved.Dispose();
        labelWithSubheader.Dispose();
        labelAuto.Dispose();
        
        Console.WriteLine("=== BeepLabel Test Complete ===");
    }
}