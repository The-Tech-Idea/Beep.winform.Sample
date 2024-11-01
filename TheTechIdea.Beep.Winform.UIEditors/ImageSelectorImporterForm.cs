using System;
using System.Collections;
using System.Collections.Generic;

using System.Data;
using System.Resources;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;


namespace TheTechIdea.Beep.Winform.Controls.UIEditors
{

    // Form use to  return the selected image Path either from local or project resources
    // Or just Import Images wether is local or project resources and supply the path
    // can import image types bmp,jpg,jpeg,png,svg
    public partial class ImageSelectorImporterForm : Form
    {
        private Control _previewControl;
        public string SelectedImagePath { get; private set; }
        public ImageSelectorImporterForm()
        {
            InitializeComponent();
            LoadResourceFiles();
            // Add event handlers
            LocalResoucesradioButton.CheckedChanged += (s, e) => RefreshResources();
            ProjectResoucesradioButton.CheckedChanged += (s, e) => RefreshResources();
            ImportLocalResourcesbutton.Click += (s, e) => ImportResource();
            ProjectResourceImportbutton.Click += (s, e) => ImportResource();
            ImagelistBox.SelectedIndexChanged += (s, e) => PreviewResource();
            SelectImagebutton.Click += (s, e) => SelectImage();
        }
        private void InitializeBeepImagePreview()
        {
            // Use reflection to create an instance of BeepImage if available
            Type beepImageType = Type.GetType("TheTechIdea.Beep.Winform.Controls.BeepImage, TheTechIdea.Beep.Winform.Controls");
            if (beepImageType != null)
            {
                _previewControl = (Control)Activator.CreateInstance(beepImageType);
                _previewControl.Dock = DockStyle.Fill;
                panel2.Controls.Add(_previewControl);
            }
        }
        private string SelectImage()
        {
            // return the selected image Path either from local or project resources
            // close form
            return SelectedImagePath = LocalResoucesradioButton.Checked
                ? ImagelistBox.SelectedItem.ToString()
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesPathcomboBox.SelectedItem.ToString());
        }

        private void ImportResource()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.svg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    // Define the target path for resources
                    string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Properties/Resources");
                    if (!Directory.Exists(resourcesPath))
                    {
                        Directory.CreateDirectory(resourcesPath);
                    }

                    // Copy image to resources folder
                    string destPath = Path.Combine(resourcesPath, Path.GetFileName(filePath));
                    File.Copy(filePath, destPath, true);

                    // Add the image to the .resx file
                    string resxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesPathcomboBox.SelectedItem.ToString());
                    using (ResXResourceWriter resxWriter = new ResXResourceWriter(resxFile))
                    {
                        resxWriter.AddResource(fileName, new Bitmap(destPath));
                        resxWriter.Generate();
                    }

                    RefreshResources(); // Refresh the resources list
                }
            }
        }
        private void RemoveResource(string resourceName)
        {
            string resxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesPathcomboBox.SelectedItem.ToString());
            string tempResxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp.resx");

            using (ResXResourceReader reader = new ResXResourceReader(resxFile))
            using (ResXResourceWriter writer = new ResXResourceWriter(tempResxFile))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    if ((string)entry.Key != resourceName) // Exclude the resource to be removed
                    {
                        writer.AddResource(entry.Key.ToString(), entry.Value);
                    }
                }
            }

            // Replace original resx file with updated one
            File.Delete(resxFile);
            File.Move(tempResxFile, resxFile);

            RefreshResources(); // Refresh the resources list
        }
        private void LoadResourceFiles()
        {
            // Find all .resx files in the project (you may need to adapt the search path)
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] resxFiles = Directory.GetFiles(projectPath, "*.resx", SearchOption.AllDirectories);

            foreach (var resxFile in resxFiles)
            {
                ResourcesPathcomboBox.Items.Add(resxFile.Replace(projectPath, ""));
            }

            if (ResourcesPathcomboBox.Items.Count > 0)
            {
                ResourcesPathcomboBox.SelectedIndex = 0;
                RefreshResources(); // Load resources from the first resx file by default
            }
        }
        private void PreviewResource(string imagePath)
        {
            if (_previewControl != null)
            {
                var imagePathProp = _previewControl.GetType().GetProperty("ImagePath");
                imagePathProp?.SetValue(_previewControl, imagePath); // Late-set the ImagePath property
            }
        }
        private void PreviewResource()
        {
            if (LocalResoucesradioButton.Checked && ImagelistBox.SelectedItem != null)
            {
                // Load from local path directly
                string filePath = ImagelistBox.SelectedItem.ToString();
                PreviewResource( filePath);
            }
            else if (ProjectResoucesradioButton.Checked && ImagelistBox.SelectedItem != null)
            {
                // Load from .resx resource
                string resourceName = ImagelistBox.SelectedItem.ToString();
                string selectedResxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesPathcomboBox.SelectedItem.ToString());

                using (var reader = new ResXResourceReader(selectedResxFile))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        if (entry.Key.ToString() == resourceName)
                        {
                            if (entry.Value is Bitmap bitmap)
                            {
                                // Save bitmap as a temporary file and set ImagePath to the file path
                                string tempFilePath = Path.Combine(Path.GetTempPath(), $"{resourceName}.png");
                                bitmap.Save(tempFilePath, System.Drawing.Imaging.ImageFormat.Png);
                                PreviewResource(tempFilePath);
                            }
                            else if (entry.Value is string svgPath && Path.GetExtension(svgPath).Equals(".svg", StringComparison.OrdinalIgnoreCase))
                            {
                                // Handle SVG by setting ImagePath directly
                                PreviewResource(svgPath);
                            }
                            break;
                        }
                    }
                }
            }
        }


        private void RefreshResources()
        {
            ImagelistBox.Items.Clear();

            if (LocalResoucesradioButton.Checked)
            {
                LoadLocalImages();
            }
            else if (ProjectResoucesradioButton.Checked)
            {
                LoadResourcesFromResx();
            }
        }
        private void LoadLocalImages()
        {
            // Example: Load images from a specific folder
            string localImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalImages");
            if (Directory.Exists(localImagePath))
            {
                var images = Directory.GetFiles(localImagePath, "*.*").Where(file =>
                    file.EndsWith(".bmp") || file.EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png") || file.EndsWith(".svg"));
                ImagelistBox.Items.AddRange(images.ToArray());
            }
        }

        private void LoadResourcesFromResx()
        {
            string selectedResxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesPathcomboBox.SelectedItem.ToString());
            using (var reader = new ResXResourceReader(selectedResxFile))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    ImagelistBox.Items.Add(entry.Key.ToString());
                }
            }
        }
    }
}
