using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;

/// <summary>
/// Simple test to demonstrate the shared Material Design architecture working
/// </summary>
public class MaterialSizeTest
{
    public static void TestMaterialSizing()
    {
        Console.WriteLine("=== Testing Material Design Shared Architecture ===");
        
        // Test 1: Standard Material Design sizing
        var comboStandard = new BeepComboBox();
        comboStandard.EnableMaterialStyle = true;
        comboStandard.MaterialPreserveContentArea = false; // Standard Material sizing
        comboStandard.ComboBoxAutoSizeForMaterial = true;
        
        Console.WriteLine($"Test 1 - Standard Material Sizing:");
        Console.WriteLine($"Initial size: {comboStandard.Width}x{comboStandard.Height}");
        Console.WriteLine(comboStandard.GetMaterialSizeInfo());
        
        // Force size compensation
        comboStandard.ForceMaterialSizeCompensation();
        Console.WriteLine($"After compensation: {comboStandard.Width}x{comboStandard.Height}");
        
        Console.WriteLine();
        
        // Test 2: Content-preserving Material Design sizing
        var comboPreserved = new BeepComboBox();
        comboPreserved.EnableMaterialStyle = true;
        comboPreserved.MaterialPreserveContentArea = true; // Content-preserving sizing
        comboPreserved.ComboBoxAutoSizeForMaterial = true;
        
        Console.WriteLine($"Test 2 - Content-Preserving Material Sizing:");
        Console.WriteLine($"Initial size: {comboPreserved.Width}x{comboPreserved.Height}");
        Console.WriteLine(comboPreserved.GetMaterialSizeInfo());
        
        // Force size compensation
        comboPreserved.ForceMaterialSizeCompensation();
        Console.WriteLine($"After compensation: {comboPreserved.Width}x{comboPreserved.Height}");
        
        Console.WriteLine();
        
        // Test 3: Material property changes trigger automatic compensation
        var comboAuto = new BeepComboBox();
        comboAuto.EnableMaterialStyle = true;
        comboAuto.MaterialAutoSizeCompensation = true;
        
        Console.WriteLine($"Test 3 - Automatic Property Change Compensation:");
        Console.WriteLine($"Before variant change: {comboAuto.Width}x{comboAuto.Height}");
        
        // Change variant - should trigger automatic compensation
        comboAuto.MaterialVariant = TheTechIdea.Beep.Winform.Controls.Models.MaterialTextFieldVariant.Filled;
        Console.WriteLine($"After changing to Filled variant: {comboAuto.Width}x{comboAuto.Height}");
        
        // Cleanup
        comboStandard.Dispose();
        comboPreserved.Dispose();
        comboAuto.Dispose();
        
        Console.WriteLine("=== Test Complete ===");
    }
}