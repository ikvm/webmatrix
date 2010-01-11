namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using Microsoft.Win32;
    using System;
    using System.Drawing;

    public sealed class MxTheme
    {
        private static bool AppThemed;
        private static bool AppThemedChecked;
        private static Brush menuBarBrush;
        private static bool UserPreferencesChangingHandlerSet;

        private MxTheme()
        {
        }

        private static void EnsureUserPreferencesListener()
        {
            if (!UserPreferencesChangingHandlerSet)
            {
                UserPreferencesChangingHandlerSet = true;
                SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(MxTheme.OnUserPreferenceChanged);
            }
        }

        private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if ((e.Category == UserPreferenceCategory.Color) && (menuBarBrush != null))
            {
                menuBarBrush.Dispose();
                menuBarBrush = null;
            }
        }

        public static bool IsAppThemed
        {
            get
            {
                if (!AppThemedChecked)
                {
                    AppThemed = (Environment.OSVersion.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0) && (Interop.IsAppThemed() != 0);
                    AppThemedChecked = true;
                }
                return AppThemed;
            }
        }

        public static Brush MenuBarBrush
        {
            get
            {
                if (!IsAppThemed)
                {
                    return SystemBrushes.Menu;
                }
                if (menuBarBrush == null)
                {
                    int sysColor = Interop.GetSysColor(30);
                    int red = sysColor & 0xff;
                    int green = (sysColor >> 8) & 0xff;
                    int blue = (sysColor >> 0x10) & 0xff;
                    menuBarBrush = new SolidBrush(Color.FromArgb(0xff, red, green, blue));
                    EnsureUserPreferencesListener();
                }
                return menuBarBrush;
            }
        }
    }
}

