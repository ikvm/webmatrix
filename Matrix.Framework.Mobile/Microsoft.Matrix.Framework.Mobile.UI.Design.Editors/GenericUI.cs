namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.Drawing;

    internal class GenericUI
    {
        internal static readonly Bitmap DeleteIcon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "Delete.ico").ToBitmap();
        internal static readonly Bitmap ErrorIcon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "Error.ico").ToBitmap();
        internal static readonly Bitmap InfoIcon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "Info.ico").ToBitmap();
        internal static readonly Bitmap SortDownIcon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "SortDown.ico").ToBitmap();
        internal static readonly Bitmap SortUpIcon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "SortUp.ico").ToBitmap();
    }
}

