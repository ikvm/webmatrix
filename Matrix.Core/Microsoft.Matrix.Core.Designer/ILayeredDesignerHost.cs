namespace Microsoft.Matrix.Core.Designer
{
    using System;
    using System.ComponentModel.Design;

    public interface ILayeredDesignerHost
    {
        IDesignLayer AddLayer(IServiceProvider serviceProvider, string name, IDesigner parentDesigner, bool clearOnActivate);
        void RemoveLayer(IDesignLayer layer);

        IDesignLayer ActiveLayer { get; set; }
    }
}

