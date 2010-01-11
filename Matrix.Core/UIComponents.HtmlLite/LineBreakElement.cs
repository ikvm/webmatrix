namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System.Drawing;

    public sealed class LineBreakElement : VisualElement
    {
        protected override Size Measure(ElementRenderData renderData)
        {
            return new Size(0, 1);
        }

        public override Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode LayoutMode
        {
            get
            {
                return Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode.LineBreakAfter;
            }
        }
    }
}

