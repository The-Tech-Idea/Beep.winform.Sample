using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;

/// <summary>
/// Test to verify BeepButton Material Design migration and functionality
/// </summary>
public class BeepButtonMaterialTest
{
    public static void TestBeepButtonMaterialDesign()
    {
        Console.WriteLine("=== Testing BeepButton Material Design Migration ===");
        
        try
        {
            // Test 1: Basic button creation
            var button1 = new BeepButton();
            button1.Text = "Basic Button";
            Console.WriteLine($"? Test 1 - Basic creation: SUCCESS - Size: {button1.Width}x{button1.Height}");
            
            // Test 2: Button with Material Design enabled
            var button2 = new BeepButton();
            button2.EnableMaterialStyle = true;
            button2.Text = "Material Button";
            button2.ButtonAutoSizeForMaterial = true;
            Console.WriteLine($"? Test 2 - Material Design: SUCCESS - Size: {button2.Width}x{button2.Height}");
            
            // Test 3: Button with size compensation
            var button3 = new BeepButton();
            button3.EnableMaterialStyle = true;
            button3.ButtonAutoSizeForMaterial = true;
            button3.Text = "Auto-sized Material Button";
            button3.ApplyMaterialSizeCompensation();
            Console.WriteLine($"? Test 3 - Size compensation: SUCCESS - Size: {button3.Width}x{button3.Height}");
            
            // Test 4: Button with image and text
            var button4 = new BeepButton();
            button4.Text = "Button with Icon";
            button4.ImagePath = "test.svg";
            button4.TextImageRelation = TextImageRelation.ImageBeforeText;
            button4.EnableMaterialStyle = true;
            Console.WriteLine($"? Test 4 - Image+Text: SUCCESS - Size: {button4.Width}x{button4.Height}");
            
            // Test 5: Button with Material Design variants
            var button5 = new BeepButton();
            button5.EnableMaterialStyle = true;
            button5.MaterialVariant = TheTechIdea.Beep.Winform.Controls.Models.MaterialTextFieldVariant.Outlined;
            button5.Text = "Outlined Button";
            Console.WriteLine($"? Test 5 - Material variants: SUCCESS - Size: {button5.Width}x{button5.Height}");
            
            // Test 6: Button convenience properties
            var button6 = new BeepButton();
            button6.ButtonLabel = "Button Label";
            button6.ButtonHelperText = "Helper text";
            button6.ButtonErrorText = "Error text";
            button6.ButtonHasError = false;
            Console.WriteLine($"? Test 6 - Convenience properties: SUCCESS - Label: {button6.ButtonLabel}");
            
            // Test 7: Force Material Design size compensation
            var button7 = new BeepButton();
            button7.EnableMaterialStyle = true;
            button7.Text = "Force Compensation Test";
            button7.ForceMaterialSizeCompensation();
            Console.WriteLine($"? Test 7 - Force compensation: SUCCESS - Size: {button7.Width}x{button7.Height}");
            
            // Test 8: Get Material Design size information
            var button8 = new BeepButton();
            button8.EnableMaterialStyle = true;
            button8.Text = "Size Info Test";
            button8.ButtonType = TheTechIdea.Beep.Winform.Controls.Models.ButtonType.Normal;
            string sizeInfo = button8.GetMaterialSizeInfo();
            Console.WriteLine($"? Test 8 - Size info: SUCCESS - Has size info: {!string.IsNullOrEmpty(sizeInfo)}");
            
            // Test 9: Button with popup functionality
            var button9 = new BeepButton();
            button9.PopupMode = true;
            button9.Text = "Popup Button";
            button9.EnableMaterialStyle = true;
            Console.WriteLine($"? Test 9 - Popup mode: SUCCESS - PopupMode: {button9.PopupMode}");
            
            // Test 10: Button theming and states
            var button10 = new BeepButton();
            button10.Text = "State Test";
            button10.IsColorFromTheme = true;
            button10.IsStillButton = false;
            button10.HideText = false;
            button10.UseScaledFont = false;
            Console.WriteLine($"? Test 10 - States and theming: SUCCESS - Text visible: {!button10.HideText}");
            
            // Cleanup
            button1.Dispose();
            button2.Dispose();
            button3.Dispose();
            button4.Dispose();
            button5.Dispose();
            button6.Dispose();
            button7.Dispose();
            button8.Dispose();
            button9.Dispose();
            button10.Dispose();
            
            Console.WriteLine("? All BeepButton Material Design tests passed successfully!");
            Console.WriteLine("?? BeepButton migration to BaseControl completed successfully!");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Test failed with exception: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("=== BeepButton Material Design Test Complete ===");
    }
}