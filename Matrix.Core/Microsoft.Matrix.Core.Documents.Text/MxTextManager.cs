namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Drawing;

    internal sealed class MxTextManager : TextManager
    {
        private const string PrintFontFacePreference = "PrintFontFace";
        private const string PrintFontSizePreference = "PrintFontSize";
        private const string SourceFontFacePreference = "SourceFontFace";
        private const string SourceFontSizePreference = "SourceFontSize";

        public MxTextManager(IServiceProvider provider) : base(provider)
        {
            IPreferencesService service = (IPreferencesService) base.ServiceProvider.GetService(typeof(IPreferencesService));
            string familyName = "Lucida Console";
            int num = 8;
            string str2 = "Lucida Console";
            int num2 = 8;
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(MxTextManager));
                if (preferencesStore != null)
                {
                    familyName = preferencesStore.GetValue("SourceFontFace", "Lucida Console");
                    num = preferencesStore.GetValue("SourceFontSize", 8);
                    str2 = preferencesStore.GetValue("PrintFontFace", "Lucida Console");
                    num2 = preferencesStore.GetValue("PrintFontSize", 8);
                }
            }
            try
            {
                base.SourceFont = new Font(familyName, (float) num);
            }
            catch
            {
                base.SourceFont = new Font("Lucida Console", 8f);
            }
            try
            {
                base.PrintFont = new Font(str2, (float) num2);
            }
            catch
            {
                base.PrintFont = new Font("Lucida Console", 8f);
            }
        }

        protected override void Dispose()
        {
            IPreferencesService service = (IPreferencesService) base.ServiceProvider.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(MxTextManager));
                preferencesStore.SetValue("SourceFontFace", base.SourceFont.Name, "Lucida Console");
                preferencesStore.SetValue("SourceFontSize", (int) base.SourceFont.Size, 8);
                preferencesStore.SetValue("PrintFontFace", base.PrintFont.Name, "Lucida Console");
                preferencesStore.SetValue("PrintFontSize", (int) base.PrintFont.Size, 8);
            }
            base.Dispose();
        }

        internal void UpdateLanguageSettings()
        {
            foreach (TextControl control in base.TextControls)
            {
                if (control is SourceView)
                {
                    ((SourceView) control).UpdateLanguageSettings();
                }
            }
        }
    }
}

