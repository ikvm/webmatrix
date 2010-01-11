namespace Microsoft.Matrix.Plugins.CodeWizards.Web
{
    using Microsoft.Matrix.Core.Plugins;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Drawing;
    using System.IO;
    using System.Web.Mail;
    using System.Windows.Forms;

    internal sealed class MailMessageCodeWizardForm : CodeWizardForm
    {
        private string _generatedCode;
        private MailMessageWizardPanel _mailMessagePanel;

        public MailMessageCodeWizardForm(MailMessageCodeWizard codeWizard) : base(codeWizard)
        {
            this.InitializeComponent();
            this._mailMessagePanel = new MailMessageWizardPanel(base.ServiceProvider);
            base.WizardPanels.Add(this._mailMessagePanel);
        }

        private CodeStatement BuildAssignment(string propertyName, string value)
        {
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("mailMessage");
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(targetObject, propertyName);
            return new CodeAssignStatement(left, new CodePrimitiveExpression(value));
        }

        private CodeStatement BuildComment(string comment)
        {
            return new CodeCommentStatement(comment);
        }

        private CodeStatement BuildConstructor()
        {
            CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeof(MailMessage), "mailMessage");
            CodeExpression[] parameters = new CodeExpression[0];
            CodeObjectCreateExpression expression = new CodeObjectCreateExpression(typeof(MailMessage), parameters);
            statement.InitExpression = expression;
            return statement;
        }

        private CodeStatement BuildFormatAssignment(MailFormat format)
        {
            CodeVariableReferenceExpression targetObject = new CodeVariableReferenceExpression("mailMessage");
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(targetObject, "BodyFormat");
            CodeTypeReferenceExpression expression3 = new CodeTypeReferenceExpression(typeof(MailFormat));
            string fieldName = "Text";
            if (format == MailFormat.Html)
            {
                fieldName = "Html";
            }
            return new CodeAssignStatement(left, new CodeFieldReferenceExpression(expression3, fieldName));
        }

        private CodeStatement BuildSetSmtpServer(string smtpServer)
        {
            CodeTypeReferenceExpression targetObject = new CodeTypeReferenceExpression(typeof(SmtpMail));
            CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(targetObject, "SmtpServer");
            return new CodeAssignStatement(left, new CodePrimitiveExpression(smtpServer));
        }

        private CodeStatement BuildSmtpMailSend()
        {
            CodeTypeReferenceExpression targetObject = new CodeTypeReferenceExpression(typeof(SmtpMail));
            CodeVariableReferenceExpression expression2 = new CodeVariableReferenceExpression("mailMessage");
            return new CodeExpressionStatement(new CodeMethodInvokeExpression(targetObject, "Send", new CodeExpression[] { expression2 }));
        }

        private void InitializeComponent()
        {
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(460, 0xf2);
            base.Icon = null;
            base.Name = "MailMessageCodeWizardForm";
            this.Text = "Send Email Message Code Wizard";
            base.ResumeLayout(false);
        }

        protected override void OnCompleted()
        {
            ICodeGenerator generator = base.CodeDomProvider.CreateGenerator();
            StringWriter w = new StringWriter();
            CodeGeneratorOptions o = new CodeGeneratorOptions();
            generator.GenerateCodeFromStatement(new CodeSnippetStatement(string.Empty), w, o);
            generator.GenerateCodeFromStatement(this.BuildComment(" Build a MailMessage"), w, o);
            generator.GenerateCodeFromStatement(this.BuildConstructor(), w, o);
            string fromText = this._mailMessagePanel.FromText;
            if (fromText.Length == 0)
            {
                generator.GenerateCodeFromStatement(this.BuildComment(" TODO: Set the mailMessage.From property"), w, o);
            }
            else
            {
                generator.GenerateCodeFromStatement(this.BuildAssignment("From", fromText), w, o);
            }
            string toText = this._mailMessagePanel.ToText;
            if (toText.Length == 0)
            {
                generator.GenerateCodeFromStatement(this.BuildComment(" TODO: Set the mailMessage.To property"), w, o);
            }
            else
            {
                generator.GenerateCodeFromStatement(this.BuildAssignment("To", toText), w, o);
            }
            generator.GenerateCodeFromStatement(this.BuildAssignment("Subject", this._mailMessagePanel.SubjectText), w, o);
            generator.GenerateCodeFromStatement(this.BuildFormatAssignment(this._mailMessagePanel.MailFormat), w, o);
            generator.GenerateCodeFromStatement(new CodeSnippetStatement(string.Empty), w, o);
            generator.GenerateCodeFromStatement(this.BuildComment(" TODO: Set the mailMessage.Body property"), w, o);
            generator.GenerateCodeFromStatement(new CodeSnippetStatement(string.Empty), w, o);
            string smtpServerText = this._mailMessagePanel.SmtpServerText;
            if (smtpServerText.Length == 0)
            {
                generator.GenerateCodeFromStatement(this.BuildComment(" TODO: Set the System.Web.SmtpMail.SmtpServer property"), w, o);
            }
            else
            {
                generator.GenerateCodeFromStatement(this.BuildSetSmtpServer(smtpServerText), w, o);
            }
            generator.GenerateCodeFromStatement(this.BuildSmtpMailSend(), w, o);
            generator.GenerateCodeFromStatement(new CodeSnippetStatement(string.Empty), w, o);
            this._generatedCode = w.ToString();
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public override string GeneratedCode
        {
            get
            {
                if (this._generatedCode == null)
                {
                    return string.Empty;
                }
                return this._generatedCode;
            }
        }
    }
}

