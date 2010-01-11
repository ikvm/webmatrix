namespace Microsoft.Matrix.Core.Services
{
    using Microsoft.Matrix.Utility;
    using System;

    public interface IComponentGalleryService
    {
        Library BrowseGallery(string typeFilter);
    }
}

