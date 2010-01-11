namespace Microsoft.Matrix.Plugins.CodeWizards.Web
{
    using Microsoft.Matrix.Core.Plugins;

    [Plugin("Send Email Message", "Constructs an email message and sends it using System.Web.Mail.")]
    public sealed class MailMessageCodeWizard : CodeWizard
    {
        protected override CodeWizardForm CreateCodeWizardForm()
        {
            return new MailMessageCodeWizardForm(this);
        }
    }
}

