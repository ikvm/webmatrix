namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Security.Permissions;
    using System.Web.UI.Design;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms.Design;

    internal abstract class MobileComponentEditorPage : ComponentEditorPage
    {
        private MobileControl _control = null;
        private ControlDesigner _designer = null;
        private IHelpService _helpService = null;
        private ISite _site = null;

        protected MobileComponentEditorPage()
        {
        }

        protected MobileControl GetBaseControl()
        {
            if (this._control == null)
            {
                IComponent selectedComponent = base.GetSelectedComponent();
                this._control = (MobileControl) selectedComponent;
            }
            return this._control;
        }

        protected ControlDesigner GetBaseDesigner()
        {
            if (this._designer == null)
            {
                IDesignerHost service = (IDesignerHost) this.DesignerSite.GetService(typeof(IDesignerHost));
                this._designer = (ControlDesigner) service.GetDesigner(base.GetSelectedComponent());
            }
            return this._designer;
        }

        public override void ShowHelp()
        {
            if (this.HelpService != null)
            {
                this.HelpService.ShowHelpFromKeyword(this.HelpKeyword);
            }
        }

        public override bool SupportsHelp()
        {
            return (this.HelpService != null);
        }

        protected ISite DesignerSite
        {
            get
            {
                if (this._site == null)
                {
                    IComponent selectedComponent = base.GetSelectedComponent();
                    this._site = selectedComponent.Site;
                }
                return this._site;
            }
        }

        protected abstract string HelpKeyword { get; }

        private IHelpService HelpService
        {
            get
            {
                if (this._helpService == null)
                {
                    this._helpService = (IHelpService) this.DesignerSite.GetService(typeof(IHelpService));
                }
                return this._helpService;
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        protected class LoadingModeResource : IDisposable
        {
            private MobileComponentEditorPage _page;

            internal LoadingModeResource(MobileComponentEditorPage page)
            {
                this._page = page;
                this._page.EnterLoadingMode();
            }

            public void Dispose()
            {
                this._page.ExitLoadingMode();
            }
        }
    }
}

