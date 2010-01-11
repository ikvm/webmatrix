namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Drawing;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public sealed class CommandToolWindow : ToolWindow
    {
        private CommandPrompt _commandPrompt;

        public CommandToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Panel panel = new Panel();
            this._commandPrompt = new CommandPrompt();
            panel.SuspendLayout();
            base.SuspendLayout();
            this._commandPrompt.BorderStyle = BorderStyle.None;
            this._commandPrompt.Dock = DockStyle.Fill;
            this._commandPrompt.ForeColor = Color.Cyan;
            this._commandPrompt.BackColor = Color.Black;
            this._commandPrompt.Font = new Font("Lucida Console", 8f);
            this._commandPrompt.TabIndex = 0;
            panel.BackColor = SystemColors.ControlDark;
            panel.Controls.Add(this._commandPrompt);
            panel.Dock = DockStyle.Fill;
            panel.DockPadding.All = 1;
            panel.TabIndex = 0;
            base.Controls.Add(panel);
            this.Text = "Command Shell";
            base.Icon = new Icon(typeof(CommandToolWindow), "CommandToolWindow.ico");
            panel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private sealed class CommandPrompt : MxRichTextBox
        {
            private ArrayList _commandHistory;
            private Process _commandProcess;
            private int _commandStartIndex;
            private Thread _errorThread;
            private int _historyIndex;
            private bool _inCommandEditMode;
            private bool _isErrorMode;
            private bool _isExiting;
            private StringBuilder _outputBuffer;
            private Thread _outputThread;

            public CommandPrompt()
            {
                base.DetectUrls = false;
                this.Multiline = true;
                base.HideSelection = false;
                base.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
                base.WordWrap = false;
                base.ShowSelectionMargin = true;
                this._historyIndex = -1;
                this._commandHistory = new ArrayList();
            }

            private void CleanCommandProcess()
            {
                Application.Idle -= new EventHandler(this.OnApplicationIdle);
                if (this._commandProcess != null)
                {
                    if (!this._commandProcess.HasExited)
                    {
                        try
                        {
                            this._commandProcess.Kill();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    this._commandProcess = null;
                }
                this._isErrorMode = false;
                this._errorThread = null;
                this._outputThread = null;
            }

            private void ClearCurrentCommand()
            {
                base.BeginUpdate();
                try
                {
                    if (this.CommandStartIndex != this.TextLength)
                    {
                        this.Text = this.Text.Substring(0, this.CommandStartIndex);
                        base.SelectionStart = this.TextLength;
                        base.ScrollToCaret();
                        base.ClearUndo();
                    }
                }
                finally
                {
                    this._inCommandEditMode = false;
                    base.EndUpdate(true);
                }
            }

            private void CommandShellErrorReaderThread()
            {
                Process process = this._commandProcess;
                do
                {
                    char ch = (char) process.StandardError.Read();
                    this._isErrorMode = true;
                    if (ch != '\0')
                    {
                        lock (this._outputBuffer)
                        {
                            this._outputBuffer.Append(ch);
                        }
                    }
                    if (process.StandardError.Peek() == -1)
                    {
                        this._isErrorMode = false;
                    }
                }
                while (!process.HasExited);
                this._isErrorMode = false;
            }

            private void CommandShellOutputReaderThread()
            {
                Process process = this._commandProcess;
                do
                {
                    string str = process.StandardOutput.ReadLine();
                    while (this._isErrorMode)
                    {
                        Thread.Sleep(250);
                    }
                    lock (this._outputBuffer)
                    {
                        this._outputBuffer.Append(str);
                        this._outputBuffer.Append(Environment.NewLine);
                    }
                }
                while (!process.HasExited);
            }

            private bool CreateCommandProcess()
            {
                this._outputBuffer = new StringBuilder(0x100);
                this._commandProcess = new Process();
                this._commandProcess.EnableRaisingEvents = true;
                this._commandProcess.Exited += new EventHandler(this.OnProcessExited);
                this._commandProcess.StartInfo.CreateNoWindow = true;
                this._commandProcess.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
                this._commandProcess.StartInfo.Arguments = "/Q /E:ON /F:OFF /D";
                this._commandProcess.StartInfo.UseShellExecute = false;
                this._commandProcess.StartInfo.RedirectStandardInput = true;
                this._commandProcess.StartInfo.RedirectStandardOutput = true;
                this._commandProcess.StartInfo.RedirectStandardError = true;
                this._commandProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                bool flag = this._commandProcess.Start();
                if (flag)
                {
                    this._outputThread = new Thread(new ThreadStart(this.CommandShellOutputReaderThread));
                    this._outputThread.Name = "Cmd.StdOut";
                    this._outputThread.IsBackground = true;
                    this._outputThread.Start();
                    this._errorThread = new Thread(new ThreadStart(this.CommandShellErrorReaderThread));
                    this._errorThread.Name = "Cmd.StdErr";
                    this._errorThread.IsBackground = true;
                    this._errorThread.Start();
                    Application.Idle += new EventHandler(this.OnApplicationIdle);
                }
                return flag;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this._isExiting = true;
                    this.CleanCommandProcess();
                }
                base.Dispose(disposing);
            }

            private string GetHistoryCommand()
            {
                if (this._historyIndex < 0)
                {
                    return null;
                }
                return (string) this._commandHistory[this._historyIndex];
            }

            private bool MoveNextHistoryCommand()
            {
                if (this._historyIndex < (this._commandHistory.Count - 1))
                {
                    this._historyIndex++;
                    return true;
                }
                return false;
            }

            private bool MovePreviousHistoryCommand()
            {
                if (this._historyIndex > 0)
                {
                    this._historyIndex--;
                    return true;
                }
                return false;
            }

            private void OnApplicationIdle(object sender, EventArgs e)
            {
                string text = null;
                lock (this._outputBuffer)
                {
                    if (this._outputBuffer.Length > 0)
                    {
                        text = this._outputBuffer.ToString();
                        this._outputBuffer.Length = 0;
                    }
                }
                if ((text != null) && (text.Length != 0))
                {
                    int num = text.LastIndexOf('\f');
                    if (num != -1)
                    {
                        base.Clear();
                        text = text.Substring(num + 1);
                    }
                    base.AppendText(text);
                    base.ClearUndo();
                }
            }

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                this.CreateCommandProcess();
                base.BeginInvoke(new MethodInvoker(this.SetFocus));
            }

            private void OnProcessExited(object sender, EventArgs e)
            {
                if (!this._isExiting)
                {
                    this.CleanCommandProcess();
                    base.Clear();
                    this.CreateCommandProcess();
                }
            }

            protected override bool ProcessKeyMessage(ref Message m)
            {
                if (m.Msg != 0x100)
                {
                    if (m.Msg == 0x102)
                    {
                        switch (((Keys) ((int) m.WParam)))
                        {
                            case Keys.Back:
                                if (base.SelectionStart > this.CommandStartIndex)
                                {
                                    goto Label_0192;
                                }
                                return true;

                            case Keys.Tab:
                            case Keys.Return:
                                return true;
                        }
                        if (base.SelectionStart >= this.CommandStartIndex)
                        {
                            this._inCommandEditMode = true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    switch (((Keys) ((int) m.WParam)))
                    {
                        case Keys.Back:
                        case Keys.Left:
                            if (base.SelectionStart > this.CommandStartIndex)
                            {
                                goto Label_0192;
                            }
                            return true;

                        case Keys.Tab:
                        case Keys.Prior:
                        case Keys.Next:
                            return true;

                        case Keys.Return:
                        {
                            if (base.SelectionStart < this.CommandStartIndex)
                            {
                                this.SelectionLength = 0;
                                base.SelectionStart = this.TextLength;
                                goto Label_0192;
                            }
                            string command = this.Text.Substring(this.CommandStartIndex);
                            this.SendCommand(command, true);
                            return true;
                        }
                        case Keys.Escape:
                            if (this._inCommandEditMode)
                            {
                                this.ClearCurrentCommand();
                            }
                            return true;

                        case Keys.End:
                            base.SelectionStart = this.TextLength;
                            return true;

                        case Keys.Home:
                            base.SelectionStart = this.CommandStartIndex;
                            return true;

                        case Keys.Up:
                            if (this.MovePreviousHistoryCommand())
                            {
                                this.SetCurrentCommand(this.GetHistoryCommand());
                            }
                            return true;

                        case Keys.Down:
                            if (this.MoveNextHistoryCommand())
                            {
                                this.SetCurrentCommand(this.GetHistoryCommand());
                            }
                            return true;
                    }
                    if (base.SelectionStart < this.CommandStartIndex)
                    {
                        return true;
                    }
                }
            Label_0192:
                return base.ProcessKeyMessage(ref m);
            }

            private void SendCommand(string command, bool updateHistory)
            {
                string strA = command.Trim();
                if ((string.Compare(strA, "exit", true) == 0) || (string.Compare(strA, "more", true) == 0))
                {
                    this.ClearCurrentCommand();
                }
                else
                {
                    if (updateHistory)
                    {
                        this.UpdateHistory(command);
                    }
                    this._inCommandEditMode = false;
                    base.AppendText(Environment.NewLine);
                    base.ClearUndo();
                    base.SelectionStart = this.TextLength;
                    base.ScrollToCaret();
                    this._inCommandEditMode = false;
                    this._commandProcess.StandardInput.WriteLine(command);
                    this._commandProcess.StandardInput.Flush();
                }
            }

            private void SetCurrentCommand(string command)
            {
                base.BeginUpdate();
                try
                {
                    this.Text = this.Text.Substring(0, this.CommandStartIndex) + command;
                    base.SelectionStart = this.TextLength;
                    base.ScrollToCaret();
                    base.ClearUndo();
                }
                finally
                {
                    this._inCommandEditMode = true;
                    base.EndUpdate(true);
                }
            }

            private void SetFocus()
            {
                base.Focus();
            }

            private void UpdateHistory(string command)
            {
                command = command.Trim();
                if (command.Length != 0)
                {
                    this._commandHistory.Add(command);
                    this._historyIndex = this._commandHistory.Count;
                }
            }

            private int CommandStartIndex
            {
                get
                {
                    if (!this._inCommandEditMode)
                    {
                        this._commandStartIndex = this.TextLength;
                    }
                    return this._commandStartIndex;
                }
            }
        }
    }
}

