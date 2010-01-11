namespace Microsoft.Matrix.Core.UserInterface
{
    using System;

    internal sealed class LanguageOptionsPage : OptionsPage
    {
        public LanguageOptionsPage(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        public override void CommitChanges()
        {
        }

        private void InitializeComponent()
        {
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }

        public override string OptionsPath
        {
            get
            {
                return @"Text Editor\Languages\(General)";
            }
        }
    }
}

