namespace Microsoft.Matrix.Core.Services
{
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class ComponentGalleryService : IComponentGalleryService, IDisposable
    {
        private IServiceProvider _serviceProvider;
        private const int blockSize = 0x400;

        public ComponentGalleryService(IServiceProvider provider)
        {
            this._serviceProvider = provider;
        }

        Library IComponentGalleryService.BrowseGallery(string typeFilter)
        {
            ComponentGalleryDialog form = new ComponentGalleryDialog(this._serviceProvider);
            form.TypeFilter = typeFilter;
            IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            if (service.ShowDialog(form) == DialogResult.OK)
            {
                return form.DownloadedLibrary;
            }
            return null;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }
    }
}

