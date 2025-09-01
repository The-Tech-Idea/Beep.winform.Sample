using System;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;

/// <summary>
/// Simple test form to verify BaseControl-based controls are visible
/// </summary>
public partial class VisibilityTestForm : Form
{
    private BeepButton testButton;
    private BeepComboBox testComboBox;
    private BeepLabel testLabel;
    
    public VisibilityTestForm()
    {
        InitializeComponent();
        CreateTestControls();
    }
    
    private void InitializeComponent()
    {
        this.SuspendLayout();
        
        // Basic form setup
        this.Text = "BaseControl Visibility Test";
        this.Size = new Size(600, 400);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;
        
        this.ResumeLayout(false);
    }
    
    private void CreateTestControls()
    {
        try
        {
            Console.WriteLine("=== Creating Test Controls ===");
            
            // Test 1: Create BeepButton
            testButton = new BeepButton();
            testButton.Text = "Test Button - VISIBLE?";
            testButton.Location = new Point(50, 50);
            testButton.Size = new Size(200, 40);
            testButton.BackColor = Color.LightBlue;
            testButton.EnableMaterialStyle = false; // Disable Material Design for initial test
            this.Controls.Add(testButton);
            Console.WriteLine($"? BeepButton created: Location={testButton.Location}, Size={testButton.Size}, Visible={testButton.Visible}");
            
            // Test 2: Create BeepComboBox
            testComboBox = new BeepComboBox();
            testComboBox.Location = new Point(50, 120);
            testComboBox.Size = new Size(200, 40);
            testComboBox.BackColor = Color.LightGreen;
            testComboBox.EnableMaterialStyle = false; // Disable Material Design for initial test
            this.Controls.Add(testComboBox);
            Console.WriteLine($"? BeepComboBox created: Location={testComboBox.Location}, Size={testComboBox.Size}, Visible={testComboBox.Visible}");
            
            // Test 3: Create BeepLabel
            testLabel = new BeepLabel();
            testLabel.Text = "Test Label - VISIBLE?";
            testLabel.Location = new Point(50, 190);
            testLabel.Size = new Size(200, 40);
            testLabel.BackColor = Color.LightYellow;
            testLabel.EnableMaterialStyle = false; // Disable Material Design for initial test
            this.Controls.Add(testLabel);
            Console.WriteLine($"? BeepLabel created: Location={testLabel.Location}, Size={testLabel.Size}, Visible={testLabel.Visible}");
            
            // Test 4: Add status label
            var statusLabel = new Label();
            statusLabel.Text = "? Controls created successfully! They should be visible now.";
            statusLabel.ForeColor = Color.Green;
            statusLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            statusLabel.Location = new Point(50, 250);
            statusLabel.Size = new Size(400, 30);
            this.Controls.Add(statusLabel);
            
            // Test 5: Add Material Design test button
            var materialButton = new BeepButton();
            materialButton.Text = "Material Design Test";
            materialButton.Location = new Point(300, 50);
            materialButton.Size = new Size(200, 40);
            materialButton.EnableMaterialStyle = true;
            materialButton.BackColor = Color.LightCoral;
            this.Controls.Add(materialButton);
            Console.WriteLine($"? Material BeepButton created: Location={materialButton.Location}, Size={materialButton.Size}");
            
            Console.WriteLine("? All test controls created successfully!");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Test control creation failed: {ex.Message}");
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
    
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        
        Console.WriteLine("=== Form Shown - Checking Control Visibility ===");
        
        foreach (Control control in this.Controls)
        {
            if (control is BeepButton || control is BeepComboBox || control is BeepLabel)
            {
                Console.WriteLine($"Control: {control.GetType().Name} - Location: {control.Location}, Size: {control.Size}, Visible: {control.Visible}, DrawingRect: {(control as dynamic)?.DrawingRect}");
            }
        }
    }
}

/// <summary>
/// Application entry point for testing
/// </summary>
public static class VisibilityTestProgram
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        // Run the visibility test
        Console.WriteLine("Starting BaseControl Visibility Test...");
        Application.Run(new VisibilityTestForm());
    }
}