namespace Microsoft.Matrix.Packages.Code.JSharp
{
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;

    public class JSharpTextControlHost : ITextControlHost, IDisposable
    {
        private TextControl _control;
        private ArrayList _handlerList;

        public JSharpTextControlHost(TextControl control)
        {
            this._control = control;
        }

        public void Dispose()
        {
            if (this._handlerList != null)
            {
                for (int i = this._handlerList.Count - 1; i >= 0; i--)
                {
                    ((IDisposable) this._handlerList[i]).Dispose();
                }
                this._handlerList.Clear();
            }
            this._control = null;
        }

        public void OnTextViewCreated(TextView view)
        {
            this.HandlerList.Add(new BlockIndentTextViewCommandHandler(view));
            this.HandlerList.Add(new JSharpTextViewCommandHandler(view));
        }

        private ArrayList HandlerList
        {
            get
            {
                if (this._handlerList == null)
                {
                    this._handlerList = new ArrayList();
                }
                return this._handlerList;
            }
        }
    }
}

