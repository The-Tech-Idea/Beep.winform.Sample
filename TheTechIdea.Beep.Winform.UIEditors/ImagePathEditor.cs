using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;


namespace TheTechIdea.Beep.Winform.Controls.UIEditors
{
    [CLSCompliant(false)]
    public class ImagePathEditor : UITypeEditor
    {
        public ImagePathEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            MessageBox.Show("SimpleEditor invoked!");  // Just a quick visual check
            return base.EditValue(context, provider, value);
            //string path = value as string;
            //IWindowsFormsEditorService editorService = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
            //if (editorService != null)
            //{
            //    using (var form = new ImageSelectorImporterForm())
            //    {
            //        if (form.ShowDialog() == DialogResult.OK)
            //        {
            //            path=form.SelectedImagePath;
            //        }
            //    }
            
            
            //}
            //return path;
        }
        
    }
}
