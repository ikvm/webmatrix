namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class UIService : IMxUIService, IUIService, IDisposable
    {
        private MxApplicationWindow _appWindow;
        private int _modalDialogCount;
        private string _newDialogFontFace;
        private int _newDialogFontSize;
        private IServiceProvider _serviceProvider;
        private HybridDictionary _uiPreferences;
        private const string DefaultDialogFontFace = "Tahoma";
        private const int DefaultDialogFontSize = 8;
        private const string DialogFontFacePreference = "DialogFontFace";
        private const string DialogFontFacePreferenceKey = "NewDialogFontFace";
        private const string DialogFontSizePreference = "DialogFontSize";
        private const string DialogFontSizePreferenceKey = "NewDialogFontSize";

        public UIService(IServiceProvider serviceProvider, MxApplicationWindow appWindow)
        {
            this._serviceProvider = serviceProvider;
            this._appWindow = appWindow;
        }

        private void EnableModelessUI(bool enable)
        {
            if (enable)
            {
                if (this._modalDialogCount > 0)
                {
                    this._modalDialogCount--;
                }
            }
            else
            {
                this._modalDialogCount++;
            }
            if (this._serviceProvider != null)
            {
                ICommandManager service = (ICommandManager) this._serviceProvider.GetService(typeof(ICommandManager));
                if (service != null)
                {
                    if (enable)
                    {
                        service.ResumeCommandUpdate();
                    }
                    else
                    {
                        service.SuspendCommandUpdate();
                    }
                }
            }
        }

        void IMxUIService.ReportError(Exception exception, string caption, bool isWarning)
        {
            string message;
            if (exception == null)
            {
                message = string.Empty;
            }
            else
            {
                message = exception.Message;
            }
            ((IMxUIService) this).ReportError(message, caption, isWarning);
        }

        void IMxUIService.ReportError(string error, string caption, bool isWarning)
        {
            MessageBoxIcon icon = isWarning ? MessageBoxIcon.Exclamation : MessageBoxIcon.Hand;
            this.ShowMessage(error, caption, icon, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
        }

        DialogResult IMxUIService.ShowDialog(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }
            DialogResult cancel = DialogResult.Cancel;
            try
            {
                this.EnableModelessUI(false);
                cancel = form.ShowDialog(this.DialogOwner);
            }
            finally
            {
                this.EnableModelessUI(true);
            }
            return cancel;
        }

        void IMxUIService.ShowMessage(string text, string caption)
        {
            this.ShowMessage(text, caption, MessageBoxIcon.Asterisk, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
        }

        DialogResult IMxUIService.ShowMessage(string text, string caption, MessageBoxIcon icon, MessageBoxButtons buttons)
        {
            return this.ShowMessage(text, caption, icon, buttons, MessageBoxDefaultButton.Button1);
        }

        DialogResult IMxUIService.ShowMessage(string text, string caption, MessageBoxIcon icon, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return this.ShowMessage(text, caption, icon, buttons, defaultButton);
        }

        private DialogResult ShowMessage(string message, string caption, MessageBoxIcon icon, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            DialogResult cancel = DialogResult.Cancel;
            this.EnableModelessUI(false);
            try
            {
                IApplicationIdentity service = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
                string title = service.Title;
                string text = string.Empty;
                if ((caption != null) && (caption.Length != 0))
                {
                    text = caption;
                    if ((message != null) && (message.Length != 0))
                    {
                        text = text + "\r\n\r\n";
                    }
                }
                text = text + message;
                cancel = MessageBox.Show(this.DialogOwner, text, title, buttons, icon, defaultButton);
            }
            finally
            {
                this.EnableModelessUI(true);
            }
            return cancel;
        }

        void IDisposable.Dispose()
        {
            IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(UIService));
                IDictionary styles = this.Styles;
                preferencesStore.SetValue("DialogFontFace", this._newDialogFontFace, "Tahoma");
                preferencesStore.SetValue("DialogFontSize", this._newDialogFontSize, 8);
            }
            this._appWindow = null;
            this._serviceProvider = null;
        }

        bool IUIService.CanShowComponentEditor(object component)
        {
            bool flag = false;
            if (component is ICustomTypeDescriptor)
            {
                component = ((ICustomTypeDescriptor) component).GetPropertyOwner(null);
            }
            try
            {
                if (TypeDescriptor.GetEditor(component, typeof(ComponentEditor)) != null)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        IWin32Window IUIService.GetDialogOwnerWindow()
        {
            return this.DialogOwner;
        }

        void IUIService.SetUIDirty()
        {
        }

        bool IUIService.ShowComponentEditor(object component, IWin32Window parent)
        {
            bool flag = false;
            if (component is ICustomTypeDescriptor)
            {
                component = ((ICustomTypeDescriptor) component).GetPropertyOwner(null);
            }
            try
            {
                ComponentEditor editor = (ComponentEditor) TypeDescriptor.GetEditor(component, typeof(ComponentEditor));
                if (editor == null)
                {
                    return false;
                }
                WindowsFormsComponentEditor editor2 = editor as WindowsFormsComponentEditor;
                if (editor2 != null)
                {
                    parent = ((IUIService) this).GetDialogOwnerWindow();
                    return editor2.EditComponent(component, parent);
                }
                flag = editor.EditComponent(component);
            }
            catch
            {
            }
            return flag;
        }

        DialogResult IUIService.ShowDialog(Form form)
        {
            return ((IMxUIService) this).ShowDialog(form);
        }

        void IUIService.ShowError(Exception ex)
        {
            ((IMxUIService) this).ReportError(ex, "Error", false);
        }

        void IUIService.ShowError(string message)
        {
            ((IMxUIService) this).ReportError((Exception) null, message, false);
        }

        void IUIService.ShowError(Exception ex, string message)
        {
            ((IMxUIService) this).ReportError(ex, message, false);
        }

        void IUIService.ShowMessage(string message)
        {
            this.ShowMessage(message, null, MessageBoxIcon.Asterisk, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
        }

        void IUIService.ShowMessage(string message, string caption)
        {
            this.ShowMessage(message, caption, MessageBoxIcon.Asterisk, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
        }

        DialogResult IUIService.ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return this.ShowMessage(message, caption, MessageBoxIcon.Asterisk, buttons, MessageBoxDefaultButton.Button1);
        }

        bool IUIService.ShowToolWindow(Guid toolWindow)
        {
            return false;
        }

        //IWin32Window IMxUIService.DialogOwner
        //{
        //    get
        //    {
        //        if (this._modalDialogCount > 0)
        //        {
        //            return new WindowHandle(Interop.GetActiveWindow());
        //        }
        //        return this._appWindow;
        //    }
        //}

        //Font IMxUIService.UIFont
        //{
        //    get
        //    {
        //        IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
        //        string familyName = "Tahoma";
        //        int num = 8;
        //        if (service != null)
        //        {
        //            PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(UIService));
        //            if (preferencesStore != null)
        //            {
        //                familyName = preferencesStore.GetValue("DialogFontFace", "Tahoma");
        //                this._newDialogFontFace = familyName;
        //                num = preferencesStore.GetValue("DialogFontSize", 8);
        //                this._newDialogFontSize = num;
        //            }
        //        }
        //        try
        //        {
        //            return new Font(familyName, (float) num);
        //        }
        //        catch
        //        {
        //            return Control.DefaultFont;
        //        }
        //    }
        //}

        IDictionary IUIService.Styles
        {
            get
            {
                if (this._uiPreferences == null)
                {
                    this._uiPreferences = new HybridDictionary();
                    this._uiPreferences["DialogFont"] = this.UIFont;
                }
                return this._uiPreferences;
            }
        }

        #region IMxUIService 成员


        IWin32Window IMxUIService.DialogOwner
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IMxUIService 成员


        Font IMxUIService.UIFont
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}

