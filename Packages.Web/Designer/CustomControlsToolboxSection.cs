namespace Microsoft.Matrix.Packages.Web.Designer
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class CustomControlsToolboxSection : WebFormsToolboxSection
    {
        public CustomControlsToolboxSection(string name) : base(name)
        {
        }

        private CustomControlsToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }
    }
}

