namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal sealed class AddFileDialog : MxForm
    {
        private MxButton _cancelButton;
        private ListBox _categoryListBox;
        private CheckBox _classInfoCheckBox;
        private MxLabel _classLabel;
        private MxTextBox _classText;
        private GroupLabel _codeGroup;
        private ArrayList _codeLanguages;
        private TemplateListViewItem _currentTemplateItem;
        private CodeDocumentLanguage _defaultLanguage;
        private MxLabel _descriptionLabel;
        private MxTextBox _fileNameText;
        private TrackButton _iconViewButton;
        private bool _internalChange;
        private MxComboBox _languageCombo;
        private Label _languageLabel;
        private TrackButton _listViewButton;
        private string _location;
        private MxButton _locationPickerButton;
        private MxTextBox _locationText;
        private MxLabel _namespaceLabel;
        private MxTextBox _namespaceText;
        private DocumentProjectItem _newProjectItem;
        private MxButton _okButton;
        private Project _project;
        private HybridDictionary _templateCategories;
        private ListView _templateListView;
        private const int CSharpIndex = 0;
        private const int VisualBasicIndex = 1;

        public AddFileDialog(Project project) : this(project, null, false)
        {
        }

        public AddFileDialog(Project project, string location, bool fixedLocation) : base(project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            this.InitializeComponent();
            Bitmap bitmap = new Bitmap(typeof(AddFileDialog), "IconView.bmp");
            bitmap.MakeTransparent(Color.Fuchsia);
            this._iconViewButton.EnabledImage = bitmap;
            bitmap = new Bitmap(typeof(AddFileDialog), "ListView.bmp");
            bitmap.MakeTransparent(Color.Fuchsia);
            this._listViewButton.EnabledImage = bitmap;
            this._project = project;
            if (location != null)
            {
                if (fixedLocation)
                {
                    this._location = location;
                    this._locationText.ReadOnly = true;
                }
                this._locationText.Text = location;
            }
            this._codeLanguages = new ArrayList();
            ILanguageManager manager = (ILanguageManager) this.GetService(typeof(ILanguageManager));
            if (manager != null)
            {
                foreach (DocumentLanguage language in manager.DocumentLanguages)
                {
                    CodeDocumentLanguage language2 = language as CodeDocumentLanguage;
                    if (language2 != null)
                    {
                        this._codeLanguages.Add(language2);
                    }
                }
                this._defaultLanguage = (CodeDocumentLanguage) manager.DefaultCodeLanguage;
            }
            IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(AddFileDialog));
                if ((preferencesStore != null) && (preferencesStore.GetValue("View", 0) != 0))
                {
                    this._listViewButton.Checked = true;
                    this._iconViewButton.Checked = false;
                    this._templateListView.View = View.List;
                }
            }
        }

        private CodeDocumentLanguage GetCodeLanguage()
        {
            CodeDocumentLanguage language = this._defaultLanguage;
            int selectedIndex = this._languageCombo.SelectedIndex;
            if (selectedIndex != -1)
            {
                language = (CodeDocumentLanguage) this._codeLanguages[selectedIndex];
            }
            return language;
        }

        private string GetCodeLanguageFileExtension()
        {
            CodeDocumentLanguage codeLanguage = this.GetCodeLanguage();
            if (codeLanguage == null)
            {
                return string.Empty;
            }
            string fileExtension = codeLanguage.CodeDomProvider.FileExtension;
            if (fileExtension.StartsWith("."))
            {
                fileExtension = fileExtension.Substring(1);
            }
            return fileExtension;
        }

        private string GetFileName()
        {
            string codeLanguageFileExtension;
            string strA = this._fileNameText.Text.Trim();
            if (strA.Length == 0)
            {
                return string.Empty;
            }
            if ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.IsCode) != TemplateFlags.None)
            {
                codeLanguageFileExtension = this.GetCodeLanguageFileExtension();
            }
            else
            {
                codeLanguageFileExtension = this._currentTemplateItem.DocumentType.Extension.ToLower();
            }
            string strB = "." + codeLanguageFileExtension;
            if ((strA.Length > strB.Length) && (string.Compare(strA, strA.Length - strB.Length, strB, 0, strB.Length, true) == 0))
            {
                return strA;
            }
            if (strA[strA.Length - 1] == '.')
            {
                return (strA + codeLanguageFileExtension);
            }
            return (strA + strB);
        }

        private void InitializeComponent()
        {
            Panel panel = new Panel();
            Panel panel2 = new Panel();
            Panel panel3 = new Panel();
            MxLabel label = new MxLabel();
            MxLabel label2 = new MxLabel();
            MxLabel label3 = new MxLabel();
            this._templateListView = new ListView();
            this._descriptionLabel = new MxLabel();
            this._categoryListBox = new ListBox();
            this._fileNameText = new MxTextBox();
            this._locationText = new MxTextBox();
            this._codeGroup = new GroupLabel();
            this._languageLabel = new MxLabel();
            this._languageCombo = new MxComboBox();
            this._locationPickerButton = new MxButton();
            this._classInfoCheckBox = new MxCheckBox();
            this._classLabel = new MxLabel();
            this._classText = new MxTextBox();
            this._namespaceLabel = new MxLabel();
            this._namespaceText = new MxTextBox();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._iconViewButton = new TrackButton();
            this._listViewButton = new TrackButton();
            panel.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            base.SuspendLayout();
            label.Location = new Point(12, 8);
            label.Size = new Size(0x40, 0x10);
            label.TabIndex = 0;
            label.Text = "&Templates:";
            this._categoryListBox.IntegralHeight = false;
            this._categoryListBox.Dock = DockStyle.Fill;
            this._categoryListBox.BorderStyle = BorderStyle.None;
            this._categoryListBox.ScrollAlwaysVisible = true;
            this._categoryListBox.TabIndex = 0;
            this._categoryListBox.Sorted = true;
            this._categoryListBox.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedCategoryListBox);
            panel.BackColor = SystemColors.ControlDark;
            panel.DockPadding.All = 1;
            panel.Location = new Point(12, 0x1a);
            panel.Size = new Size(0xb9, 0xa8);
            panel.TabIndex = 1;
            panel.Controls.Add(this._categoryListBox);
            this._iconViewButton.Size = new Size(0x12, 0x12);
            this._iconViewButton.Location = new Point(0x19f, 6);
            this._iconViewButton.TabIndex = 2;
            this._iconViewButton.TabStop = false;
            this._iconViewButton.Checked = true;
            this._iconViewButton.Click += new EventHandler(this.OnClickIconViewButton);
            this._listViewButton.Size = new Size(0x12, 0x12);
            this._listViewButton.Location = new Point(0x1b2, 6);
            this._listViewButton.TabIndex = 3;
            this._listViewButton.TabStop = false;
            this._listViewButton.Click += new EventHandler(this.OnClickListViewButton);
            this._templateListView.BorderStyle = BorderStyle.None;
            this._templateListView.Dock = DockStyle.Fill;
            this._templateListView.HideSelection = false;
            this._templateListView.MultiSelect = false;
            this._templateListView.TabIndex = 0;
            this._templateListView.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedTemplateListView);
            panel2.BackColor = SystemColors.ControlDark;
            panel2.Controls.Add(this._templateListView);
            panel2.DockPadding.All = 1;
            panel2.Location = new Point(200, 0x1a);
            panel2.Size = new Size(0xfc, 0xa8);
            panel2.TabIndex = 4;
            this._descriptionLabel.BackColor = SystemColors.Control;
            this._descriptionLabel.Dock = DockStyle.Fill;
            this._descriptionLabel.TabIndex = 0;
            panel3.BackColor = SystemColors.ControlDark;
            panel3.Controls.Add(this._descriptionLabel);
            panel3.DockPadding.All = 1;
            panel3.Location = new Point(12, 0xc6);
            panel3.Size = new Size(440, 0x12);
            panel3.TabIndex = 5;
            label3.Location = new Point(12, 230);
            label3.Size = new Size(0x34, 0x10);
            label3.TabIndex = 6;
            label3.Text = "&Location:";
            this._locationText.Location = new Point(0x52, 0xe4);
            this._locationText.Size = new Size(0x156, 20);
            this._locationText.TabIndex = 7;
            this._locationText.FlatAppearance = true;
            this._locationText.AlwaysShowFocusCues = true;
            this._locationText.TextChanged += new EventHandler(this.OnTextChangedFields);
            this._locationPickerButton.Location = new Point(0x1ac, 0xe3);
            this._locationPickerButton.Size = new Size(0x18, 0x16);
            this._locationPickerButton.TabIndex = 8;
            this._locationPickerButton.Text = "...";
            this._locationPickerButton.Click += new EventHandler(this.OnClickPickerButton);
            label2.Location = new Point(12, 0xfe);
            label2.Size = new Size(0x34, 0x10);
            label2.TabIndex = 9;
            label2.Text = "&Filename:";
            this._fileNameText.Location = new Point(0x52, 0xfc);
            this._fileNameText.Size = new Size(0x156, 20);
            this._fileNameText.TabIndex = 10;
            this._fileNameText.Text = "";
            this._fileNameText.FlatAppearance = true;
            this._fileNameText.AlwaysShowFocusCues = true;
            this._fileNameText.TextChanged += new EventHandler(this.OnTextChangedFields);
            this._codeGroup.Location = new Point(12, 0x116);
            this._codeGroup.Size = new Size(0x1b7, 0x10);
            this._codeGroup.TabIndex = 11;
            this._codeGroup.Text = "Code";
            this._codeGroup.Visible = false;
            this._languageLabel.Location = new Point(0x10, 0x12b);
            this._languageLabel.Size = new Size(0x40, 0x10);
            this._languageLabel.TabIndex = 12;
            this._languageLabel.Text = "L&anguage:";
            this._languageLabel.Visible = false;
            this._languageCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._languageCombo.FlatAppearance = true;
            this._languageCombo.AlwaysShowFocusCues = true;
            this._languageCombo.Location = new Point(0x52, 0x127);
            this._languageCombo.Size = new Size(130, 0x15);
            this._languageCombo.TabIndex = 13;
            this._languageCombo.Visible = false;
            this._languageCombo.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedLanguageCombo);
            this._classInfoCheckBox.Location = new Point(0x10, 320);
            this._classInfoCheckBox.Size = new Size(200, 0x10);
            this._classInfoCheckBox.TabIndex = 14;
            this._classInfoCheckBox.Visible = false;
            this._classInfoCheckBox.CheckedChanged += new EventHandler(this.OnCheckedChangedClassInfoCheckBox);
            this._classLabel.Location = new Point(0x10, 340);
            this._classLabel.Size = new Size(0x40, 0x10);
            this._classLabel.TabIndex = 15;
            this._classLabel.Text = "&Class:";
            this._classLabel.Visible = false;
            this._classText.Location = new Point(0x52, 0x152);
            this._classText.Size = new Size(130, 20);
            this._classText.TabIndex = 0x10;
            this._classText.Text = "";
            this._classText.FlatAppearance = true;
            this._classText.AlwaysShowFocusCues = true;
            this._classText.Visible = false;
            this._classText.TextChanged += new EventHandler(this.OnTextChangedFields);
            this._namespaceLabel.Location = new Point(230, 340);
            this._namespaceLabel.Size = new Size(0x40, 0x10);
            this._namespaceLabel.TabIndex = 0x11;
            this._namespaceLabel.Text = "&Namespace:";
            this._namespaceLabel.Visible = false;
            this._namespaceText.Location = new Point(320, 0x152);
            this._namespaceText.Size = new Size(130, 20);
            this._namespaceText.TabIndex = 0x12;
            this._namespaceText.Text = "";
            this._namespaceText.FlatAppearance = true;
            this._namespaceText.AlwaysShowFocusCues = true;
            this._namespaceText.Visible = false;
            this._namespaceText.TextChanged += new EventHandler(this.OnTextChangedFields);
            this._okButton.Location = new Point(0x12a, 0x170);
            this._okButton.TabIndex = 0x13;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnClickOKButton);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x17a, 0x170);
            this._cancelButton.TabIndex = 20;
            this._cancelButton.Text = "Cancel";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1d2, 400);
            base.Controls.AddRange(new Control[] { 
                this._cancelButton, this._okButton, this._namespaceText, this._namespaceLabel, this._classText, this._classLabel, this._classInfoCheckBox, this._locationPickerButton, this._languageCombo, this._languageLabel, this._codeGroup, label3, this._locationText, this._fileNameText, label2, panel3, 
                panel2, this._listViewButton, this._iconViewButton, panel, label
             });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.AcceptButton = this._okButton;
            base.CancelButton = this._cancelButton;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Add New File";
            base.Icon = new Icon(typeof(AddFileDialog), "AddFileDialog.ico");
            panel.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadTemplates()
        {
            IDocumentTypeManager service = (IDocumentTypeManager) this.GetService(typeof(IDocumentTypeManager));
            TemplateCategory category = new TemplateCategory("(General)");
            this._templateCategories = new HybridDictionary(true);
            this._templateCategories.Add("General", category);
            ICollection creatableDocumentTypes = service.CreatableDocumentTypes;
            if ((creatableDocumentTypes != null) && (creatableDocumentTypes.Count != 0))
            {
                ImageList list = new ImageList();
                ImageList list2 = new ImageList();
                list.ImageSize = new Size(0x20, 0x20);
                list.ColorDepth = ColorDepth.Depth32Bit;
                this._templateListView.LargeImageList = list;
                list2.ImageSize = new Size(0x10, 0x10);
                list2.ColorDepth = ColorDepth.Depth32Bit;
                this._templateListView.SmallImageList = list2;
                ImageList.ImageCollection images = list.Images;
                ImageList.ImageCollection images2 = list2.Images;
                foreach (DocumentType type in creatableDocumentTypes)
                {
                    images.Add(type.LargeIcon);
                    images2.Add(type.SmallIcon);
                    TemplateListViewItem item = new TemplateListViewItem(type, images.Count - 1);
                    string templateCategory = type.TemplateCategory;
                    if ((templateCategory == null) || (templateCategory.Length == 0))
                    {
                        category.Items.Add(item);
                        continue;
                    }
                    TemplateCategory category2 = (TemplateCategory) this._templateCategories[templateCategory];
                    if (category2 != null)
                    {
                        category2.Items.Add(item);
                    }
                    else
                    {
                        category2 = new TemplateCategory(templateCategory);
                        category2.Items.Add(item);
                        this._templateCategories.Add(templateCategory, category2);
                    }
                }
            }
            foreach (TemplateCategory category3 in this._templateCategories.Values)
            {
                this._categoryListBox.Items.Add(category3);
            }
            this._categoryListBox.SelectedItem = category;
            if (this._fileNameText.Enabled)
            {
                this._fileNameText.Focus();
            }
        }

        private void OnCheckedChangedClassInfoCheckBox(object sender, EventArgs e)
        {
            if (!this._internalChange)
            {
                bool flag = this._classInfoCheckBox.Checked;
                this._classLabel.Visible = flag;
                this._classText.Visible = flag;
                this._namespaceLabel.Visible = flag;
                this._namespaceText.Visible = flag;
                this.UpdateUIState();
            }
        }

        private void OnClickIconViewButton(object sender, EventArgs e)
        {
            this._templateListView.View = View.LargeIcon;
            this._iconViewButton.Checked = true;
            this._listViewButton.Checked = false;
        }

        private void OnClickListViewButton(object sender, EventArgs e)
        {
            this._templateListView.View = View.List;
            this._iconViewButton.Checked = false;
            this._listViewButton.Checked = true;
        }

        private void OnClickOKButton(object sender, EventArgs e)
        {
            IMxUIService service = (IMxUIService) this.GetService(typeof(IMxUIService));
            CodeDocumentLanguage codeLanguage = null;
            string fileName = this.GetFileName();
            string itemPath = (this._location != null) ? this._location : this._locationText.Text.Trim();
            string str3 = null;
            if (!this._project.ValidateProjectItemName(fileName))
            {
                service.ReportError("'" + fileName + "' is not a valid file name.\r\nPlease try again.", this.Text, true);
                this._fileNameText.Focus();
            }
            else if ((this._location == null) && !this._project.ValidateProjectItemPath(itemPath, true))
            {
                service.ReportError("'" + itemPath + "' is not a valid location for the new file.\r\nPlease try again.", this.Text, true);
                this._locationText.Focus();
            }
            else
            {
                try
                {
                    byte[] buffer;
                    try
                    {
                        str3 = this._project.CombinePath(itemPath, fileName);
                        if (!this._project.ProjectItemExists(str3) || (service.ShowMessage("A file with the path '" + str3 + "' already exists.\r\nClick 'Yes' to continue and overwrite the existing file.\r\nClick 'No' to pick a different file.", this.Text, MessageBoxIcon.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) != DialogResult.No))
                        {
                            goto Label_013D;
                        }
                        this._fileNameText.Focus();
                    }
                    catch
                    {
                        service.ReportError("Unable to create the specified file.\r\nPlease try again.", this.Text, true);
                        this._fileNameText.Focus();
                    }
                    return;
                Label_013D:
                    buffer = null;
                    try
                    {
                        DocumentInstanceArguments instanceArguments = null;
                        if ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.HasCode) == TemplateFlags.None)
                        {
                            instanceArguments = new DocumentInstanceArguments(fileName);
                        }
                        else
                        {
                            codeLanguage = this.GetCodeLanguage();
                            string namespaceName = string.Empty;
                            string str5 = string.Empty;
                            if (this._classInfoCheckBox.Checked)
                            {
                                namespaceName = this._namespaceText.Text.Trim();
                                str5 = this._classText.Text.Trim();
                                if (codeLanguage != null)
                                {
                                    ICodeGenerator generator = codeLanguage.CodeDomProvider.CreateGenerator();
                                    if (!generator.IsValidIdentifier(str5))
                                    {
                                        service.ReportError("'" + str5 + "' is not a valid class name.\r\nPlease try again.", this.Text, true);
                                        this._classText.Focus();
                                        return;
                                    }
                                    foreach (string str6 in namespaceName.Split(new char[] { '.' }))
                                    {
                                        if (!generator.IsValidIdentifier(str6))
                                        {
                                            service.ReportError("'" + namespaceName + "' is not a valid namespace name.\r\nPlease try again.", this.Text, true);
                                            this._namespaceText.Focus();
                                            return;
                                        }
                                    }
                                }
                            }
                            instanceArguments = new DocumentInstanceArguments(fileName, codeLanguage, namespaceName, str5);
                        }
                        buffer = this._currentTemplateItem.DocumentType.Instantiate(base.ServiceProvider, instanceArguments);
                    }
                    catch
                    {
                        service.ReportError("Unable to use the selected document template to create a new file.\r\nPlease try again.", this.Text, true);
                        buffer = null;
                    }
                    if (buffer != null)
                    {
                        DocumentProjectItem item = this._project.ParsePath(str3, true) as DocumentProjectItem;
                        if (item != null)
                        {
                            Stream stream = null;
                            try
                            {
                                stream = item.GetStream(ProjectItemStreamMode.Write);
                                if (buffer.Length != 0)
                                {
                                    stream.Write(buffer, 0, buffer.Length);
                                }
                                this._newProjectItem = item;
                            }
                            finally
                            {
                                if (stream != null)
                                {
                                    stream.Close();
                                    stream = null;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    service.ReportError(exception.Message, this.Text, false);
                }
                if (this._newProjectItem != null)
                {
                    IPreferencesService service2 = (IPreferencesService) this.GetService(typeof(IPreferencesService));
                    if (service2 != null)
                    {
                        PreferencesStore preferencesStore = service2.GetPreferencesStore(typeof(AddFileDialog));
                        if (preferencesStore != null)
                        {
                            if (this._location == null)
                            {
                                preferencesStore.SetValue("Location", itemPath, string.Empty);
                            }
                            preferencesStore.SetValue("View", (this._templateListView.View == View.List) ? 1 : 0, 0);
                        }
                    }
                    if (codeLanguage != null)
                    {
                        ILanguageManager manager = (ILanguageManager) this.GetService(typeof(ILanguageManager));
                        if (manager != null)
                        {
                            ((LanguageManager) manager).SetCurrentCodeLanguage(codeLanguage);
                        }
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                else
                {
                    this._fileNameText.Focus();
                }
            }
        }

        private void OnClickPickerButton(object sender, EventArgs e)
        {
            FolderBrowser browser = new FolderBrowser();
            browser.Style = FolderBrowserStyles.RestrictToFileSystem;
            browser.StartLocation = FolderBrowserLocation.MyComputer;
            browser.Description = "Select the file location:";
            browser.Style = FolderBrowserStyles.ShowTextBox | FolderBrowserStyles.RestrictToFileSystem;
            if (browser.ShowDialog(this) == DialogResult.OK)
            {
                string directoryPath = browser.DirectoryPath;
                if (((directoryPath != null) && (directoryPath.Length != 0)) && Path.IsPathRooted(directoryPath))
                {
                    this._locationText.Text = directoryPath;
                }
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this.LoadTemplates();
        }

        private void OnSelectedIndexChangedCategoryListBox(object sender, EventArgs e)
        {
            this._currentTemplateItem = null;
            this._templateListView.Items.Clear();
            TemplateCategory selectedItem = this._categoryListBox.SelectedItem as TemplateCategory;
            if (selectedItem != null)
            {
                foreach (ListViewItem item in selectedItem.Items)
                {
                    this._templateListView.Items.Add(item);
                }
                if (selectedItem.Items.Count != 0)
                {
                    this._templateListView.Items[0].Selected = true;
                }
            }
        }

        private void OnSelectedIndexChangedLanguageCombo(object sender, EventArgs e)
        {
            if (!this._internalChange)
            {
                if ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.IsCode) != TemplateFlags.None)
                {
                    string str = this._fileNameText.Text.Trim();
                    if (str.Length != 0)
                    {
                        string codeLanguageFileExtension = this.GetCodeLanguageFileExtension();
                        this._internalChange = true;
                        try
                        {
                            int num = str.LastIndexOf('.');
                            if (num >= 0)
                            {
                                this._fileNameText.Text = str.Substring(0, num + 1) + codeLanguageFileExtension;
                            }
                            else
                            {
                                this._fileNameText.Text = str + "." + codeLanguageFileExtension;
                            }
                        }
                        finally
                        {
                            this._internalChange = false;
                        }
                    }
                }
                this.UpdateUIState();
            }
        }

        private void OnSelectedIndexChangedTemplateListView(object sender, EventArgs e)
        {
            this._currentTemplateItem = null;
            this._internalChange = true;
            try
            {
                this._fileNameText.Clear();
                this._classInfoCheckBox.Checked = false;
                this._classInfoCheckBox.Enabled = false;
                this._classLabel.Visible = false;
                this._classText.Clear();
                this._classText.Visible = false;
                this._namespaceLabel.Visible = false;
                this._namespaceText.Clear();
                this._namespaceText.Visible = false;
            }
            finally
            {
                this._internalChange = false;
            }
            bool flag = false;
            bool flag2 = false;
            if (this._templateListView.SelectedItems.Count != 0)
            {
                this._currentTemplateItem = (TemplateListViewItem) this._templateListView.SelectedItems[0];
                this._internalChange = true;
                try
                {
                    string codeLanguageFileExtension;
                    if ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.IsCode) != TemplateFlags.None)
                    {
                        codeLanguageFileExtension = this.GetCodeLanguageFileExtension();
                    }
                    else
                    {
                        codeLanguageFileExtension = this._currentTemplateItem.DocumentType.Extension.ToLower();
                    }
                    this._fileNameText.Text = this._currentTemplateItem.DocumentType.TemplateInstanceFileName + "." + codeLanguageFileExtension;
                    if ((this._location == null) && (this._locationText.Text.Trim().Length == 0))
                    {
                        PreferencesStore store;
                        string folderPath = string.Empty;
                        IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
                        if (service.GetPreferencesStore(typeof(AddFileDialog), out store))
                        {
                            folderPath = store.GetValue("Location", string.Empty);
                        }
                        if (folderPath.Length == 0)
                        {
                            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        }
                        this._locationText.Text = folderPath;
                        this._locationText.Select(0, 0);
                    }
                    if (this._languageCombo.Items.Count == 0)
                    {
                        this._languageCombo.Items.AddRange(this._codeLanguages.ToArray());
                    }
                    flag = (this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.HasCode) != TemplateFlags.None;
                    if ((this._languageCombo.SelectedIndex == -1) && flag)
                    {
                        this._languageCombo.SelectedItem = this._defaultLanguage;
                    }
                    if (flag)
                    {
                        flag2 = (this._currentTemplateItem.DocumentType.TemplateFlags & (TemplateFlags.CodeRequiresClassName | TemplateFlags.CodeRequiresNamespace)) != TemplateFlags.None;
                        if (flag2)
                        {
                            this._classInfoCheckBox.Checked = true;
                            this._classLabel.Visible = true;
                            this._classText.Visible = true;
                            this._namespaceLabel.Visible = true;
                            this._namespaceText.Visible = true;
                        }
                        else
                        {
                            this._classInfoCheckBox.Enabled = true;
                        }
                    }
                }
                finally
                {
                    this._internalChange = false;
                }
            }
            if (flag)
            {
                if (flag2)
                {
                    this._classInfoCheckBox.Text = "&Specify class information";
                }
                else
                {
                    this._classInfoCheckBox.Text = "&Specify optional class information";
                }
            }
            this._codeGroup.Visible = flag;
            this._languageLabel.Visible = flag;
            this._languageCombo.Visible = flag;
            this._classInfoCheckBox.Visible = flag;
            this.UpdateUIState();
        }

        private void OnTextChangedFields(object sender, EventArgs e)
        {
            if (!this._internalChange)
            {
                this.UpdateUIState();
            }
        }

        private void UpdateUIState()
        {
            bool flag = this._currentTemplateItem != null;
            if (flag)
            {
                this._descriptionLabel.Text = this._currentTemplateItem.DocumentType.CreateNewDescription;
            }
            this._fileNameText.Enabled = flag;
            if (this._location == null)
            {
                this._locationText.Enabled = flag;
                this._locationPickerButton.Enabled = flag;
            }
            else
            {
                this._locationText.ReadOnly = true;
                this._locationText.Enabled = true;
                this._locationPickerButton.Enabled = false;
            }
            bool flag2 = flag && ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.HasCode) != TemplateFlags.None);
            this._languageCombo.Enabled = (flag2 && (this._codeLanguages != null)) && (this._codeLanguages.Count > 1);
            this._classText.Enabled = flag2;
            this._namespaceText.Enabled = flag2;
            bool flag3 = false;
            if (flag)
            {
                flag3 = (this._fileNameText.Text.Trim().Length != 0) && ((this._location != null) || (this._locationText.Text.Trim().Length != 0));
                if (flag3 && flag2)
                {
                    flag3 = this._languageCombo.SelectedIndex != -1;
                    bool flag4 = this._classInfoCheckBox.Checked;
                    if (flag3 && (flag4 || ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.CodeRequiresClassName) != TemplateFlags.None)))
                    {
                        flag3 = this._classText.Text.Trim().Length != 0;
                    }
                    if (flag3 && (flag4 || ((this._currentTemplateItem.DocumentType.TemplateFlags & TemplateFlags.CodeRequiresNamespace) != TemplateFlags.None)))
                    {
                        flag3 = this._namespaceText.Text.Trim().Length != 0;
                    }
                }
            }
            this._okButton.Enabled = flag3;
        }

        public DocumentProjectItem NewProjectItem
        {
            get
            {
                return this._newProjectItem;
            }
        }

        private class TemplateCategory
        {
            private string _category;
            private ArrayList _items;

            public TemplateCategory(string category)
            {
                this._category = category;
                this._items = new ArrayList();
            }

            public override string ToString()
            {
                return this.Category;
            }

            public string Category
            {
                get
                {
                    return this._category;
                }
            }

            public ArrayList Items
            {
                get
                {
                    return this._items;
                }
            }
        }

        private class TemplateListViewItem : ListViewItem
        {
            private Microsoft.Matrix.Core.Documents.DocumentType _docType;

            public TemplateListViewItem(Microsoft.Matrix.Core.Documents.DocumentType docType, int imageIndex) : base(docType.Name, imageIndex)
            {
                this._docType = docType;
            }

            public Microsoft.Matrix.Core.Documents.DocumentType DocumentType
            {
                get
                {
                    return this._docType;
                }
            }
        }
    }
}

