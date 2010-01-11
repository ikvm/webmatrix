namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IDeviceSpecificSelectionProvider
    {
        event EventHandler SelectionChanged;

        void AddFilter(string filterName);
        bool FilterExists(string filterName);

        string CurrentFilter { get; }

        bool DeviceSpecificSelectionProviderEnabled { get; }
    }
}

