namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class MxStatusBar : StatusBar, IStatusBar, ISupportInitialize
    {
        private ProgressStatusBarPanel _progressPanel;
        private StatusBarPanel _textPanel;

        void IStatusBar.SetProgress(int percentComplete)
        {
            if (this._progressPanel != null)
            {
                this._progressPanel.PercentComplete = percentComplete;
            }
        }

        void IStatusBar.SetText(string text)
        {
            if (this._textPanel != null)
            {
                this._textPanel.Text = text;
            }
            else
            {
                this.Text = text;
            }
        }

        protected override void OnDrawItem(StatusBarDrawItemEventArgs e)
        {
            MxStatusBarPanel panel = e.Panel as MxStatusBarPanel;
            if (panel != null)
            {
                panel.DrawPanel(e);
            }
            else
            {
                base.OnDrawItem(e);
            }
        }

        void ISupportInitialize.BeginInit()
        {
            this._textPanel = null;
            this._progressPanel = null;
        }

        void ISupportInitialize.EndInit()
        {
            if (base.ShowPanels && (base.Panels.Count != 0))
            {
                this._textPanel = base.Panels[0];
                foreach (StatusBarPanel panel in base.Panels)
                {
                    if (panel is ProgressStatusBarPanel)
                    {
                        this._progressPanel = (ProgressStatusBarPanel) panel;
                        break;
                    }
                }
            }
        }
    }
}

