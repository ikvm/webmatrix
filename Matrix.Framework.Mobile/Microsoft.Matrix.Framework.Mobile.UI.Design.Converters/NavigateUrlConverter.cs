namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Converters
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.MobileControls;

    public class NavigateUrlConverter : StringConverter
    {
        private Control FindContainer(MobileControl mc, Type containerType)
        {
            for (Control control = mc; control != null; control = control.Parent)
            {
                if (containerType.IsAssignableFrom(control.GetType()))
                {
                    return control;
                }
            }
            return null;
        }

        private Form GetContainingForm(MobileControl mc)
        {
            return (this.FindContainer(mc, typeof(Form)) as Form);
        }

        private StyleSheet GetContainingStyleSheet(MobileControl mc)
        {
            return (this.FindContainer(mc, typeof(StyleSheet)) as StyleSheet);
        }

        protected virtual ArrayList GetControls(ITypeDescriptorContext context)
        {
            ArrayList list = new ArrayList();
            MobileControl mc = null;
            IContainer service = context.Container;
            if (context.Instance is Array)
            {
                Array instance = (Array) context.Instance;
                foreach (object obj2 in instance)
                {
                    Form containingForm = this.GetContainingForm((MobileControl) obj2);
                    if (((containingForm == null) || (containingForm.ID == null)) && (this.GetContainingStyleSheet((MobileControl) obj2) == null))
                    {
                        return null;
                    }
                }
                mc = instance.GetValue(0) as MobileControl;
            }
            else
            {
                if (context.Instance is MobileControl)
                {
                    mc = (MobileControl) context.Instance;
                }
                else
                {
                    return null;
                }
                Form form2 = this.GetContainingForm(mc);
                if (form2 == null)
                {
                    if (this.GetContainingStyleSheet(mc) == null)
                    {
                        return null;
                    }
                }
                else if ((form2.ID == null) && (this.GetContainingStyleSheet(mc) == null))
                {
                    return list;
                }
            }
            if (service == null)
            {
                service = (IContainer) mc.Site.GetService(typeof(IContainer));
            }
            if (service == null)
            {
                return null;
            }
            foreach (IComponent component in service.Components)
            {
                Form form3 = component as Form;
                if (((form3 != null) && (form3.ID != null)) && (form3.ID.Length != 0))
                {
                    list.Add("#" + form3.ID);
                }
            }
            list.Sort();
            return list;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null)
            {
                return null;
            }
            ArrayList controls = this.GetControls(context);
            if (controls == null)
            {
                return null;
            }
            return new TypeConverter.StandardValuesCollection(controls);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}

