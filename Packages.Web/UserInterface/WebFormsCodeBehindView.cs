namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.UserInterface;
    using System;

    public class WebFormsCodeBehindView : CodeBehindView
    {
        public WebFormsCodeBehindView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LineNumbersVisible
        {
            get
            {
                return false;
            }
            set
            {
                base.LineNumbersVisible = value;
            }
        }
    }
}

