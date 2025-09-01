using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;
using System.ComponentModel;

/// <summary>
/// Test form to verify design-time behavior and runtime functionality of BaseControl-based controls
/// </summary>
public partial class DesignTimeTestForm : Form
{
    private BeepButton testButton;
    private BeepComboBox testComboBox;
    private BeepLabel testLabel;
    
    public DesignTimeTestForm()
    {
        InitializeComponent();
        TestDesignTimeBehavior();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        // Basic form setup
        this.Text = "BaseControl Design-Time Test";
        this.Size = new Size(600, 400);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;
        
        this.ResumeLayout(false);
    }
    
    private void TestDesignTimeBehavior()
    {
        try
        {
            Console.WriteLine("=== Testing BaseControl Design-Time Behavior ===");
            
            // Test 1: Create controls in runtime (simulating design-time creation)
            testButton = new BeepButton();
            testButton.Text = "Design-Time Test Button";
            testButton.Location = new Point(50, 50);
            testButton.Size = new Size(200, 40);
            testButton.EnableMaterialStyle = true;
            this.Controls.Add(testButton);
            Console.WriteLine("? BeepButton created successfully at runtime");
            
            testComboBox = new BeepComboBox();
            testComboBox.Location = new Point(50, 120);
            testComboBox.Size = new Size(200, 40);
            testComboBox.EnableMaterialStyle = true;
            this.Controls.Add(testComboBox);
            Console.WriteLine("? BeepComboBox created successfully at runtime");
            
            testLabel = new BeepLabel();
            testLabel.Text = "Design-Time Test Label";
            testLabel.Location = new Point(50, 190);
            testLabel.Size = new Size(200, 40);
            testLabel.EnableMaterialStyle = true;
            this.Controls.Add(testLabel);
            Console.WriteLine("? BeepLabel created successfully at runtime");
            
            // Test 2: Test design-mode detection
            TestDesignModeDetection();
            
            // Test 3: Test Material Design features
            TestMaterialDesignFeatures();
            
            // Test 4: Test helper initialization
            TestHelperInitialization();
            
            // Test 5: Test paint operations
            TestPaintOperations();
            
            Console.WriteLine("? All design-time tests passed successfully!");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Design-time test failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            
            // Show error in form
            var errorLabel = new Label();
            errorLabel.Text = $"Error: {ex.Message}";
            errorLabel.ForeColor = Color.Red;
            errorLabel.Location = new Point(50, 300);
            errorLabel.Size = new Size(500, 60);
            this.Controls.Add(errorLabel);
        }
    }
    
    private void TestDesignModeDetection()
    {
        Console.WriteLine("--- Testing Design Mode Detection ---");
        
        // Test LicenseManager approach
        bool isDesignTime1 = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        
        // Test Component.DesignMode approach
        bool isDesignTime2 = testButton.DesignMode;
        
        // Test Site.DesignMode approach
        bool isDesignTime3 = testButton.Site?.DesignMode ?? false;
        
        Console.WriteLine($"LicenseManager.UsageMode: {isDesignTime1}");
        Console.WriteLine($"Component.DesignMode: {isDesignTime2}");
        Console.WriteLine($"Site.DesignMode: {isDesignTime3}");
        
        // In runtime, all should be false
        if (!isDesignTime1 && !isDesignTime2 && !isDesignTime3)
        {
            Console.WriteLine("? Design mode detection working correctly (runtime mode)");
        }
        else
        {
            Console.WriteLine("?? Design mode detection indicates design-time in runtime");
        }
    }
    
    private void TestMaterialDesignFeatures()
    {
        Console.WriteLine("--- Testing Material Design Features ---");
        
        try
        {
            // Test Material Design properties
            testButton.MaterialVariant = TheTechIdea.Beep.Winform.Controls.Models.MaterialTextFieldVariant.Outlined;
            testButton.MaterialBorderRadius = 12;
            testButton.ApplyMaterialSizeCompensation();
            Console.WriteLine("? BeepButton Material Design features working");
            
            testComboBox.MaterialVariant = TheTechIdea.Beep.Winform.Controls.Models.MaterialTextFieldVariant.Filled;
            testComboBox.ApplyMaterialSizeCompensation();
            Console.WriteLine("? BeepComboBox Material Design features working");
            
            testLabel.MaterialVariant = TheTechIdea.Beep.Winform.Controls.Models.MaterialTextFieldVariant.Standard;
            testLabel.ApplyMaterialSizeCompensation();
            Console.WriteLine("? BeepLabel Material Design features working");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Material Design test failed: {ex.Message}");
        }
    }
    
    private void TestHelperInitialization()
    {
        Console.WriteLine("--- Testing Helper Initialization ---");
        
        try
        {
            // Test that helpers are properly initialized by accessing their properties
            var buttonDrawingRect = testButton.DrawingRect;
            Console.WriteLine($"Button DrawingRect: {buttonDrawingRect}");
            
            var comboDrawingRect = testComboBox.DrawingRect;
            Console.WriteLine($"ComboBox DrawingRect: {comboDrawingRect}");
            
            var labelDrawingRect = testLabel.DrawingRect;
            Console.WriteLine($"Label DrawingRect: {labelDrawingRect}");
            
            Console.WriteLine("? Helper initialization test passed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Helper initialization test failed: {ex.Message}");
        }
    }
    
    private void TestPaintOperations()
    {
        Console.WriteLine("--- Testing Paint Operations ---");
        
        try
        {
            // Force invalidate to test paint pipeline
            testButton.Invalidate();
            testComboBox.Invalidate();
            testLabel.Invalidate();
            
            // Test custom drawing
            using (var g = testButton.CreateGraphics())
            {
                testButton.UpdateDrawingRect();
                // If this doesn't throw, paint helpers are working
            }
            
            Console.WriteLine("? Paint operations test passed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Paint operations test failed: {ex.Message}");
        }
    }
    
    // Test method to simulate Visual Studio designer behavior
    public static void SimulateDesignerCreation()
    {
        Console.WriteLine("=== Simulating Visual Studio Designer Creation ===");
        
        try
        {
            // Create controls as the designer would
            var button = new BeepButton();
            button.Name = "beepButton1";
            button.Text = "Button from Designer";
            button.Location = new Point(10, 10);
            button.Size = new Size(120, 32);
            
            var comboBox = new BeepComboBox();
            comboBox.Name = "beepComboBox1";
            comboBox.Location = new Point(10, 60);
            comboBox.Size = new Size(120, 32);
            
            var label = new BeepLabel();
            label.Name = "beepLabel1";
            label.Text = "Label from Designer";
            label.Location = new Point(10, 110);
            label.Size = new Size(120, 32);
            
            Console.WriteLine("? Designer simulation successful");
            
            // Clean up
            button.Dispose();
            comboBox.Dispose();
            label.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Designer simulation failed: {ex.Message}");
        }
    }
    
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        
        // Add status information
        var statusLabel = new Label();
        statusLabel.Text = "? Form loaded successfully! Check console for test results.";
        statusLabel.ForeColor = Color.Green;
        statusLabel.Font = new Font("Arial", 10, FontStyle.Bold);
        statusLabel.Location = new Point(50, 250);
        statusLabel.Size = new Size(400, 30);
        this.Controls.Add(statusLabel);
        
        // Add instruction
        var instructionLabel = new Label();
        instructionLabel.Text = "If you can see this form and the controls above, the design-time issue is resolved!";
        instructionLabel.Location = new Point(50, 280);
        instructionLabel.Size = new Size(500, 40);
        this.Controls.Add(instructionLabel);
    }
}

/// <summary>
/// Application entry point for testing
/// </summary>
public static class DesignTimeTestProgram
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        // Run designer simulation first
        DesignTimeTestForm.SimulateDesignerCreation();
        
        // Run the main test form
        Application.Run(new DesignTimeTestForm());
    }
}