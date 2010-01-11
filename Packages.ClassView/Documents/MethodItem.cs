namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using System;
    using System.Reflection;
    using System.Text;

    public sealed class MethodItem : TypeDocumentItem
    {
        private bool _isConstructor;
        private string _text;

        public MethodItem(TypeDocument document, ConstructorInfo method) : base(document, method)
        {
            this._isConstructor = true;
        }

        public MethodItem(TypeDocument document, MethodInfo method) : base(document, method)
        {
        }

        private string GenerateMethodCaption(MethodBase mb)
        {
            StringBuilder builder = new StringBuilder(0x100);
            if (!this.IsConstructor)
            {
                builder.Append(mb.Name);
            }
            else
            {
                builder.Append(mb.DeclaringType.Name);
            }
            builder.Append('(');
            ParameterInfo[] parameters = mb.GetParameters();
            if (parameters != null)
            {
                int length = parameters.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i != 0)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(parameters[i].ParameterType.Name);
                }
            }
            builder.Append(')');
            return builder.ToString();
        }

        protected override TypeDocumentFilter GetVisibilityFilter()
        {
            return base.GetVisibilityFilter((MethodBase) base.Member);
        }

        public bool IsConstructor
        {
            get
            {
                return this._isConstructor;
            }
        }

        public override string Text
        {
            get
            {
                if (this._text == null)
                {
                    this._text = this.GenerateMethodCaption((MethodBase) base.Member);
                }
                return this._text;
            }
        }
    }
}

