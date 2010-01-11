namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;

    internal interface IStyle
    {
        string GetAttribute(string name);
        void ResetAttribute(string name);
        void SetAttribute(string name, string value);
    }
}

