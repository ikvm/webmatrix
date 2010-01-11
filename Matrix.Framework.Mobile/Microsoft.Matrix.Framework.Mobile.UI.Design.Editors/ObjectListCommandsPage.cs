namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Security.Permissions;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    internal sealed class ObjectListCommandsPage : ListComponentEditorPage
    {
        private ComboBox _cmbDefaultCommand;
        private ObjectList _objectList = null;
        private TextBox _txtText = null;

        public ObjectListCommandsPage()
        {
            base.Y = 0x18;
            base.CaseSensitive = false;
            base.TreeViewTitle = MobileResource.GetString("ObjectListCommandsPage_CommandNameCaption");
            base.AddButtonTitle = MobileResource.GetString("ObjectListCommandsPage_NewCommandBtnCaption");
            base.DefaultName = MobileResource.GetString("ObjectListCommandsPage_DefaultCommandName");
            base.MessageTitle = MobileResource.GetString("ObjectListCommandsPage_ErrorMessageTitle");
            base.EmptyNameMessage = MobileResource.GetString("ObjectListCommandsPage_EmptyNameError");
        }

        protected override void InitForm()
        {
            base.InitForm();
            this._objectList = (ObjectList) base.Component;
            base.CommitOnDeactivate = true;
            base.Icon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "Commands.ico");
            base.Size = new Size(0x192, 300);
            this.Text = MobileResource.GetString("ObjectListCommandsPage_Title");
            GroupLabel label = new GroupLabel();
            label.SetBounds(4, 4, 0x188, ListComponentEditorPage.LabelHeight);
            label.Text = MobileResource.GetString("ObjectListCommandsPage_CommandListGroupLabel");
            label.TabIndex = 0;
            label.TabStop = false;
            base.TreeList.TabIndex = 1;
            Label label2 = new Label();
            label2.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.LabelHeight);
            label2.Text = MobileResource.GetString("ObjectListCommandsPage_TextCaption");
            label2.TabStop = false;
            label2.TabIndex = base.TabIndex;
            this._txtText = new TextBox();
            base.Y += ListComponentEditorPage.LabelHeight;
            this._txtText.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.CmbHeight);
            this._txtText.TextChanged += new EventHandler(this.OnPropertyChanged);
            this._txtText.TabIndex = base.TabIndex + 1;
            GroupLabel label3 = new GroupLabel();
            label3.SetBounds(4, 0xee, 0x188, ListComponentEditorPage.LabelHeight);
            label3.Text = MobileResource.GetString("ObjectListCommandsPage_DataGroupLabel");
            label3.TabIndex = base.TabIndex + 2;
            label3.TabStop = false;
            Label label4 = new Label();
            label4.SetBounds(8, 260, 0xb6, ListComponentEditorPage.LabelHeight);
            label4.Text = MobileResource.GetString("ObjectListCommandsPage_DefaultCommandCaption");
            label4.TabStop = false;
            label4.TabIndex = base.TabIndex + 3;
            this._cmbDefaultCommand = new ComboBox();
            this._cmbDefaultCommand.SetBounds(8, 0x114, 0xb6, 0x40);
            this._cmbDefaultCommand.DropDownStyle = ComboBoxStyle.DropDown;
            this._cmbDefaultCommand.Sorted = true;
            this._cmbDefaultCommand.TabIndex = base.TabIndex + 4;
            this._cmbDefaultCommand.SelectedIndexChanged += new EventHandler(this.OnSetPageDirty);
            this._cmbDefaultCommand.TextChanged += new EventHandler(this.OnSetPageDirty);
            base.Controls.AddRange(new Control[] { label, label2, this._txtText, label3, label4, this._cmbDefaultCommand });
        }

        protected override void InitPage()
        {
            base.InitPage();
            this._cmbDefaultCommand.Text = this._objectList.get_DefaultCommand();
            this._txtText.Text = string.Empty;
        }

        private void LoadDefaultCommands()
        {
            this._cmbDefaultCommand.Items.Clear();
            foreach (CommandTreeNode node in base.TreeList.TvList.Nodes)
            {
                this._cmbDefaultCommand.Items.Add(node.Name);
            }
        }

        protected override void LoadItemProperties()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                if (base.CurrentNode != null)
                {
                    CommandTreeNode currentNode = (CommandTreeNode) base.CurrentNode;
                    this._txtText.Text = currentNode.Text;
                }
                else
                {
                    this._txtText.Text = string.Empty;
                }
            }
        }

        protected override void LoadItems()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                foreach (ObjectListCommand command in this._objectList.get_Commands())
                {
                    CommandTreeNode node = new CommandTreeNode(command.get_Name(), command);
                    base.TreeList.TvList.Nodes.Add(node);
                }
            }
            this.LoadDefaultCommands();
        }

        protected override void OnClickAddButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                CommandTreeNode node = new CommandTreeNode(this.GetNewName());
                base.TreeList.TvList.Nodes.Add(node);
                base.TreeList.TvList.SelectedNode = node;
                base.CurrentNode = node;
                node.Dirty = true;
                node.BeginEdit();
                this.LoadItemProperties();
                this.LoadDefaultCommands();
                this.SetDirty();
            }
        }

        protected override void OnClickRemoveButton(object source, EventArgs e)
        {
            base.OnClickRemoveButton(source, e);
            this.LoadDefaultCommands();
        }

        protected override void OnNodeRenamed()
        {
            this.LoadDefaultCommands();
        }

        protected override void OnPropertyChanged(object source, EventArgs e)
        {
            if (!base.IsLoading() && (base.CurrentNode != null))
            {
                ((CommandTreeNode) base.CurrentNode).Text = this._txtText.Text;
                this.SetDirty();
                base.CurrentNode.Dirty = true;
            }
        }

        private void OnSetPageDirty(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                this.SetDirty();
            }
        }

        protected override void SaveComponent()
        {
            base.SaveComponent();
            this._objectList.set_DefaultCommand(this._cmbDefaultCommand.Text);
            this._objectList.get_Commands().Clear();
            foreach (CommandTreeNode node in base.TreeList.TvList.Nodes)
            {
                if (node.Dirty)
                {
                    node.RuntimeCommand.set_Text(node.Text);
                    node.RuntimeCommand.set_Name(node.Name);
                }
                this._objectList.get_Commands().AddAt(-1, node.RuntimeCommand);
            }
            TypeDescriptor.Refresh(this._objectList);
        }

        protected override void UpdateControlsEnabling()
        {
            base.TreeList.TvList.Enabled = this._txtText.Enabled = base.TreeList.TvList.SelectedNode != null;
        }

        protected override string HelpKeyword
        {
            get
            {
                return "net.Mobile.ObjectListProperties.Commands";
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class CommandTreeNode : ListComponentEditorPage.ListTreeNode
        {
            private ObjectListCommand _runtimeCommand;
            private string _text;

            internal CommandTreeNode(string name) : this(name, new ObjectListCommand())
            {
            }

            internal CommandTreeNode(string name, ObjectListCommand runtimeCommand) : base(name)
            {
                this._runtimeCommand = runtimeCommand;
                this.LoadAttributes();
            }

            internal void LoadAttributes()
            {
                this._text = this._runtimeCommand.get_Text();
            }

            internal ObjectListCommand RuntimeCommand
            {
                get
                {
                    return this._runtimeCommand;
                }
            }

            internal string Text
            {
                get
                {
                    return this._text;
                }
                set
                {
                    this._text = value;
                }
            }
        }
    }
}

