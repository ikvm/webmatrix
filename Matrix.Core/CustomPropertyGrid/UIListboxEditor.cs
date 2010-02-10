namespace Kingsoft.Blaze.WorldEditor.CustomPropertyGrid
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Drawing.Design;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;
    using Kingsoft.Blaze.WorldEditor.UI.Components;

    public class UIListboxEditor : UITypeEditor
    {
        private bool bIsDropDownResizable = false;
        private ListBoxEx oList = null;
        //private object oSelectedValue = null;
        private IWindowsFormsEditorService oEditorService;
        private CustomProperty cp = null;

        public UIListboxEditor()
        { 
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                UIListboxIsDropDownResizable attribute = (UIListboxIsDropDownResizable)context.PropertyDescriptor.Attributes[typeof(UIListboxIsDropDownResizable)];
                if (attribute != null)
                {
                    bIsDropDownResizable = true;
                }
                return UITypeEditorEditStyle.DropDown;
            }
            return UITypeEditorEditStyle.None;
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return bIsDropDownResizable;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }

            oEditorService = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (oList == null)
            { 
                oList = new ListBoxEx(context, value);

                oList.Dock = DockStyle.Fill;
                oList.EditOK += new EventHandler(oList_EditOK);
                oList.EditCancel += new EventHandler(oList_EditCancel);
            }

            if (oEditorService != null)
            {
                
                //oSelectedValue = null;
                oEditorService.DropDownControl(oList);
                if (oList.SelectedValue != null)
                    return oList.SelectedValue;
            }
            else
            {
                return base.EditValue(provider, value);
            }

            return value;

        }

        void oList_EditOK(object sender, EventArgs e)
        {
            if (oEditorService != null)
                oEditorService.CloseDropDown();
        }

        void oList_EditCancel(object sender, EventArgs e)
        {
            if (oEditorService != null)
                oEditorService.CloseDropDown();
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxDatasource : Attribute
        {

            private object oDataSource;
            public UIListboxDatasource(ref object Datasource)
            {
                oDataSource = Datasource;
            }
            public object Value
            {
                get
                {
                    return oDataSource;
                }
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxValueMember : Attribute
        {

            private string sValueMember;
            public UIListboxValueMember(string ValueMember)
            {
                sValueMember = ValueMember;
            }
            public string Value
            {
                get
                {
                    return sValueMember;
                }
                set
                {
                    sValueMember = value;
                }
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxDisplayMember : Attribute
        {

            private string sDisplayMember;
            public UIListboxDisplayMember(string DisplayMember)
            {
                sDisplayMember = DisplayMember;
            }
            public string Value
            {
                get
                {
                    return sDisplayMember;
                }
                set
                {
                    sDisplayMember = value;
                }
            }

        }

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxIsDropDownResizable : Attribute
        {

        }
    }
}
