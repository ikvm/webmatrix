namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Delegate | AttributeTargets.Parameter | AttributeTargets.Interface | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Module | AttributeTargets.Assembly)]
    internal sealed class WebCategoryAttribute : CategoryAttribute
    {
        internal WebCategoryAttribute(string category) : base(category)
        {
        }

        protected override string GetLocalizedString(string value)
        {
            string localizedString = base.GetLocalizedString(value);
            if (localizedString == null)
            {
                localizedString = Microsoft.Matrix.Framework.SR.GetString("Category_" + value);
            }
            return localizedString;
        }
    }
}

