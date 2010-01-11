namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class AddInForm : TaskForm
    {
        private AddIn _addIn;
        private static Image addInGlyph;

        public AddInForm()
        {
        }

        public AddInForm(AddIn addIn) : base(addIn.ServiceProvider)
        {
            if (addIn == null)
            {
                throw new ArgumentNullException("addIn");
            }
            this._addIn = addIn;
            base.ShowInTaskbar = false;
            base.MinimizeBox = false;
            base.MaximizeBox = false;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskCaption = "Add-in";
            base.TaskGlyph = AddInGlyph;
            base.TaskAbout = true;
        }

        protected sealed override void ShowTaskAboutInformation()
        {
            IMxUIService service = (IMxUIService) this.GetService(typeof(IMxUIService));
            if (service != null)
            {
                AboutPluginDialog dialog = new AboutPluginDialog(this._addIn, "Add-in", AddInGlyph);
                service.ShowDialog(dialog);
            }
        }

        private static Image AddInGlyph
        {
            get
            {
                if (addInGlyph == null)
                {
                    addInGlyph = new Bitmap(typeof(AddInForm), "AddInFormGlyph.bmp");
                }
                return addInGlyph;
            }
        }
    }
}

