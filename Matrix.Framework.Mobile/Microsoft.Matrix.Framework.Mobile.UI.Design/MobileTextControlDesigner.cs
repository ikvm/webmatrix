namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public abstract class MobileTextControlDesigner : MobileControlDesigner
    {
        private MobileControl _textControl;

        protected MobileTextControlDesigner()
        {
        }

        protected override string GetDesignTimeNormalHtml()
        {
            HtmlMobileTextWriter designerTextWriter;
            string text = this.Text;
            Control[] array = null;
            bool flag = text.Trim().Length == 0;
            bool flag2 = this._textControl.HasControls();
            if (flag)
            {
                if (flag2)
                {
                    array = new Control[this._textControl.Controls.Count];
                    this._textControl.Controls.CopyTo(array, 0);
                }
                this.Text = "[" + this._textControl.ID + "]";
            }
            try
            {
                designerTextWriter = Utils.GetDesignerTextWriter();
                this.Adapter.Render(designerTextWriter);
            }
            finally
            {
                if (flag)
                {
                    this.Text = text;
                    if (flag2)
                    {
                        foreach (Control control in array)
                        {
                            this._textControl.Controls.Add(control);
                        }
                    }
                }
            }
            return designerTextWriter.ToString();
        }

        public override string GetPersistInnerHtml()
        {
            if (!base.IsDirty)
            {
                return null;
            }
            StringWriter sw = new StringWriter();
            this.ApplyDefaultProperties();
            bool flag = this._textControl.HasControls();
            if (((this._textControl is TextControl) || (this._textControl is TextView)) && flag)
            {
                string str = null;
                Control[] array = null;
                array = new Control[this._textControl.Controls.Count];
                this._textControl.Controls.CopyTo(array, 0);
                if (this._textControl is TextControl)
                {
                    str = ((TextControl) this._textControl).get_Text();
                    ((TextControl) this._textControl).set_Text(string.Empty);
                }
                else
                {
                    str = ((TextView) this._textControl).get_Text();
                    ((TextView) this._textControl).set_Text(string.Empty);
                }
                try
                {
                    Utils.InvokePersistInnerProperties(sw, this._textControl);
                    foreach (Control control in array)
                    {
                        Utils.InvokePersistControl(sw, control);
                    }
                }
                finally
                {
                    if (this._textControl is TextControl)
                    {
                        ((TextControl) this._textControl).set_Text(str);
                    }
                    else
                    {
                        ((TextView) this._textControl).set_Text(str);
                    }
                    foreach (Control control2 in array)
                    {
                        this._textControl.Controls.Add(control2);
                    }
                }
            }
            else
            {
                Utils.InvokePersistInnerProperties(sw, this._textControl);
            }
            base.IsDirty = false;
            this.ApplyPropertyOverrides();
            return sw.ToString();
        }

        public override void Initialize(IComponent component)
        {
            this._textControl = (MobileControl) component;
            base.Initialize(component);
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
        {
            base.OnComponentChanged(sender, ce);
            MemberDescriptor member = ce.Member;
            if ((member != null) && member.GetType().FullName.Equals("System.ComponentModel.ReflectPropertyDescriptor"))
            {
                PropertyDescriptor descriptor2 = (PropertyDescriptor) member;
                if (descriptor2.Name.Equals("Text"))
                {
                    this._textControl.Controls.Clear();
                }
            }
        }

        private string Text
        {
            get
            {
                return (this._textControl.GetType().InvokeMember("Text", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, this._textControl, null) as string);
            }
            set
            {
                this._textControl.GetType().InvokeMember("Text", BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance, null, this._textControl, new object[] { value });
            }
        }
    }
}

