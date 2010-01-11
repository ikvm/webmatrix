namespace Microsoft.Matrix.Framework
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Delegate | AttributeTargets.Parameter | AttributeTargets.Interface | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Module | AttributeTargets.Assembly)]
    internal sealed class SRDescriptionAttribute : DescriptionAttribute
    {
        private bool replaced;

        public SRDescriptionAttribute(string description) : base(description)
        {
            this.replaced = false;
        }

        public override string Description
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    base.DescriptionValue = Microsoft.Matrix.Framework.SR.GetString(base.Description);
                }
                return base.Description;
            }
        }
    }
}

