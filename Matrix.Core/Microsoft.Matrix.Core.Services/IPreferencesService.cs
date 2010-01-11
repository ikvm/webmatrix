namespace Microsoft.Matrix.Core.Services
{
    using System;
    using System.Runtime.InteropServices;

    public interface IPreferencesService
    {
        PreferencesStore GetPreferencesStore(Type storeOwnerType);
        bool GetPreferencesStore(Type storeOwnerType, out PreferencesStore preferencesStore);
        void ResetPreferencesStore(Type storeOwnerType);
    }
}

