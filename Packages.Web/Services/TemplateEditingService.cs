namespace Microsoft.Matrix.Packages.Web.Services
{
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    internal sealed class TemplateEditingService : ITemplateEditingService, IDisposable
    {
        private IServiceProvider _serviceProvider;

        public TemplateEditingService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            this._serviceProvider = null;
        }

        ITemplateEditingFrame ITemplateEditingService.CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames)
        {
            return ((ITemplateEditingService) this).CreateFrame(designer, frameName, templateNames, null, null);
        }

        ITemplateEditingFrame ITemplateEditingService.CreateFrame(TemplatedControlDesigner designer, string frameName, string[] templateNames, Style controlStyle, Style[] templateStyles)
        {
            if (designer == null)
            {
                throw new ArgumentNullException("designer");
            }
            if ((frameName == null) || (frameName.Length == 0))
            {
                throw new ArgumentNullException("frameName");
            }
            if ((templateNames == null) || (templateNames.Length == 0))
            {
                throw new ArgumentException("templateNames");
            }
            if ((templateStyles != null) && (templateStyles.Length != templateNames.Length))
            {
                throw new ArgumentException("templateStyles");
            }
            return new TemplateEditingFrame(frameName, templateNames, controlStyle, templateStyles);
        }

        string ITemplateEditingService.GetContainingTemplateName(Control control)
        {
            ITemplateDesignView service = this._serviceProvider.GetService(typeof(IDesignView)) as ITemplateDesignView;
            if (service != null)
            {
                return service.ActiveTemplateName;
            }
            return null;
        }

        bool ITemplateEditingService.SupportsNestedTemplateEditing
        {
            get
            {
                return false;
            }
        }
    }
}

