namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Web.UI.MobileControls;

    internal interface IListDesigner
    {
        void OnDataSourceChanged();

        string DataMember { get; set; }

        string DataSource { get; set; }

        string DataTextField { get; set; }

        string DataValueField { get; set; }

        MobileListItemCollection Items { get; }
    }
}

