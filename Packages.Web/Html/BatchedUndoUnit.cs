namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class BatchedUndoUnit : Interop.IOleUndoUnit
    {
        private string _description;
        private int _numUndos;
        private int _startIndex;
        private BatchedUndoType _type;
        private Interop.IOleUndoManager _undoManager;
        private Interop.IOleUndoUnit[] _undoUnits;

        internal BatchedUndoUnit(string description, Interop.IOleUndoManager undoManager, BatchedUndoType type)
        {
            this._description = description;
            this._undoManager = undoManager;
            this._type = type;
        }

        public void Close()
        {
            Interop.IEnumOleUndoUnits enumerator = this._undoManager.EnumUndoable();
            int num = this.CountUndos(enumerator);
            this._numUndos = num - this._startIndex;
            this.Pack(this._undoManager.EnumUndoable());
        }

        private int CountUndos(Interop.IEnumOleUndoUnits enumerator)
        {
            int num2 = 0;
            IntPtr zero = IntPtr.Zero;
            enumerator.Reset();
            try
            {
                int num;
                while (enumerator.Next(1, out zero, out num) == 0)
                {
                    Interop.IOleUndoUnit objectForIUnknown = (Interop.IOleUndoUnit) Marshal.GetObjectForIUnknown(zero);
                    Marshal.Release(zero);
                    if ((enumerator == null) || (num == 0))
                    {
                        return num2;
                    }
                    num2++;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Exception in CountUndos");
            }
            return num2;
        }

        public int Do(Interop.IOleUndoManager undoManager)
        {
            try
            {
                for (int i = 0; i < this._numUndos; i++)
                {
                    this._undoUnits[i].Do(undoManager);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Exception in Do");
            }
            return 0;
        }

        public int GetDescription(out string s)
        {
            s = this._description;
            return 0;
        }

        public int GetUnitType(out int clsid, out int plID)
        {
            clsid = 0;
            plID = (int) this._type;
            return 0;
        }

        public int OnNextAdd()
        {
            return 0;
        }

        public void Open()
        {
            if (this._type == BatchedUndoType.Undo)
            {
                this._startIndex = this.CountUndos(this._undoManager.EnumUndoable());
            }
            else
            {
                this._startIndex = this.CountUndos(this._undoManager.EnumRedoable());
            }
        }

        private void Pack(Interop.IEnumOleUndoUnits enumerator)
        {
            int num;
            enumerator.Reset();
            Interop.IOleUndoUnit[] unitArray = new Interop.IOleUndoUnit[this._startIndex];
            this._undoUnits = new Interop.IOleUndoUnit[this._numUndos];
            IntPtr zero = IntPtr.Zero;
            for (int i = 0; i < this._startIndex; i++)
            {
                enumerator.Next(1, out zero, out num);
                unitArray[i] = (Interop.IOleUndoUnit) Marshal.GetObjectForIUnknown(zero);
                Marshal.Release(zero);
            }
            for (int j = 0; j < this._numUndos; j++)
            {
                enumerator.Next(1, out zero, out num);
                this._undoUnits[j] = (Interop.IOleUndoUnit) Marshal.GetObjectForIUnknown(zero);
                Marshal.Release(zero);
            }
            this._undoManager.DiscardFrom(null);
            for (int k = 0; k < this._startIndex; k++)
            {
                this._undoManager.Add(unitArray[k]);
            }
            this._undoManager.Add(this);
        }
    }
}

