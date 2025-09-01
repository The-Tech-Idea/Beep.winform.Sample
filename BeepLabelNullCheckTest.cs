using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;

/// <summary>
/// Test to verify BeepLabel works properly after the null reference exception fixes
/// </summary>
public class BeepLabelNullCheckTest
{
    public static void TestBeepLabelCreation()
    {
        Console.WriteLine("=== Testing BeepLabel Creation and Initialization ===");
        
        try
        {
            // Test 1: Basic label creation
            var label1 = new BeepLabel();
            label1.Text = "Test Label";
            Console.WriteLine($"? Test 1 - Basic creation: SUCCESS - Size: {label1.Width}x{label1.Height}");
            
            // Test 2: Label with Material Design enabled
            var label2 = new BeepLabel();
            label2.EnableMaterialStyle = true;
            label2.Text = "Material Label";
            Console.WriteLine($"? Test 2 - Material Design: SUCCESS - Size: {label2.Width}x{label2.Height}");
            
            // Test 3: Label with size compensation
            var label3 = new BeepLabel();
            label3.EnableMaterialStyle = true;
            label3.LabelAutoSizeForMaterial = true;
            label3.Text = "Auto-sized Material Label";
            label3.ApplyMaterialSizeCompensation();
            Console.WriteLine($"? Test 3 - Size compensation: SUCCESS - Size: {label3.Width}x{label3.Height}");
            
            // Test 4: Label with SubHeader
            var label4 = new BeepLabel();
            label4.Text = "Main Text";
            label4.SubHeaderText = "Subheader text";
            label4.EnableMaterialStyle = true;
            Console.WriteLine($"? Test 4 - SubHeader: SUCCESS - Size: {label4.Width}x{label4.Height}");
            
            // Test 5: Resize operations (this was causing the original NullReferenceException)
            var label5 = new BeepLabel();
            label5.Size = new Size(200, 100);  // This triggers OnResize
            label5.Text = "Resized Label";
            Console.WriteLine($"? Test 5 - Resize operations: SUCCESS - Size: {label5.Width}x{label5.Height}");
            
            // Test 6: Property changes that trigger redraws
            var label6 = new BeepLabel();
            label6.Text = "Test";
            label6.BorderRadius = 5;
            label6.BackColor = Color.LightBlue;
            label6.ForeColor = Color.DarkBlue;
            Console.WriteLine($"? Test 6 - Property changes: SUCCESS - Size: {label6.Width}x{label6.Height}");
            
            // Cleanup
            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            label4.Dispose();
            label5.Dispose();
            label6.Dispose();
            
            Console.WriteLine("? All BeepLabel tests passed successfully!");
            Console.WriteLine("?? NullReferenceException issue has been resolved!");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Test failed with exception: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("=== BeepLabel Test Complete ===");
    }
}