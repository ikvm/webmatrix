namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;

    public sealed class MdiWindowManager : IWindowManager, IToolWindowManager
    {
        private DockingContainer _bottomContainer;
        private DockingContainer _leftContainer;
        private MxMainForm _mainForm;
        private DockingContainer _rightContainer;
        private DockingContainer _topContainer;

        public MdiWindowManager(MxMainForm mainForm)
        {
            if (!mainForm.IsMdiContainer)
            {
                throw new ArgumentException();
            }
            this._mainForm = mainForm;
        }

        public void EnableDocking(DockStyle dockEdge, DockingContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException();
            }
            switch (dockEdge)
            {
                case DockStyle.Top:
                    if (this._topContainer != null)
                    {
                        throw new ArgumentException();
                    }
                    this._topContainer = container;
                    break;

                case DockStyle.Bottom:
                    if (this._bottomContainer != null)
                    {
                        throw new ArgumentException();
                    }
                    this._bottomContainer = container;
                    break;

                case DockStyle.Left:
                    if (this._leftContainer != null)
                    {
                        throw new ArgumentException();
                    }
                    this._leftContainer = container;
                    break;

                case DockStyle.Right:
                    if (this._rightContainer != null)
                    {
                        throw new ArgumentException();
                    }
                    this._rightContainer = container;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            container.SetToolWindowManager(this);
        }

        private DockingContainer GetDockingContainer(DockStyle dock)
        {
            switch (dock)
            {
                case DockStyle.Top:
                    return this._topContainer;

                case DockStyle.Bottom:
                    return this._bottomContainer;

                case DockStyle.Left:
                    return this._leftContainer;

                case DockStyle.Right:
                    return this._rightContainer;
            }
            throw new ArgumentOutOfRangeException();
        }

        void IWindowManager.ActivateChildForm(MxForm childForm)
        {
            if (childForm == null)
            {
                throw new ArgumentNullException("childForm");
            }
            if (!childForm.IsMdiChild || (childForm.MdiParent != this._mainForm))
            {
                throw new ArgumentException("The specified form is not an MDI child of the window", "childForm");
            }
            childForm.Activate();
        }

        void IWindowManager.AddToolWindow(ToolWindow toolWindow, DockStyle dockEdge)
        {
            ((IWindowManager) this).AddToolWindow(toolWindow, dockEdge, -1);
        }

        void IWindowManager.AddToolWindow(ToolWindow toolWindow, DockStyle dockEdge, int toolWindowGroupIndex)
        {
            if (toolWindow == null)
            {
                throw new ArgumentNullException("toolWindow");
            }
            DockingContainer dockingContainer = this.GetDockingContainer(dockEdge);
            if (dockingContainer == null)
            {
                throw new ArgumentException("Window does not support docking on the specified edge", "dock");
            }
            dockingContainer.AddToolWindow(toolWindow, toolWindowGroupIndex);
        }

        void IWindowManager.CloseChildForm(MxForm childForm)
        {
            if (childForm == null)
            {
                throw new ArgumentNullException("childForm");
            }
            if (!childForm.IsMdiChild || (childForm.MdiParent != this._mainForm))
            {
                throw new ArgumentException("The specified form is not an MDI child of the window", "childForm");
            }
            childForm.Close();
        }

        void IWindowManager.ShowChildForm(MxForm childForm)
        {
            if (childForm == null)
            {
                throw new ArgumentNullException("childForm");
            }
            childForm.MdiParent = this._mainForm;
            childForm.Visible = true;
            childForm.Focus();
        }
    }
}

