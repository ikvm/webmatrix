namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.Design;

    internal sealed class UserControlDesignTimeHtmlGenerator
    {
        private IDictionary _projectTable;
        public static readonly UserControlDesignTimeHtmlGenerator Instance = new UserControlDesignTimeHtmlGenerator();

        private UserControlDesignTimeHtmlGenerator()
        {
            IProjectManager service = (IProjectManager) WebPackage.ServiceProvider.GetService(typeof(IProjectManager));
            if (service != null)
            {
                service.ProjectClosed += new ProjectEventHandler(this.OnProjectClosed);
            }
        }

        public void ClearDesignTimeHtml(DocumentProjectItem projectItem)
        {
            ProjectEntry entry = null;
            if (this._projectTable != null)
            {
                entry = (ProjectEntry) this._projectTable[projectItem.Project];
            }
            if (entry != null)
            {
                entry.RemoveDesignTimeHtml(projectItem);
            }
        }

        public string GetDesignTimeHtml(DocumentProjectItem projectItem, bool forceRegenerate)
        {
            ProjectEntry entry = null;
            if (this._projectTable != null)
            {
                entry = (ProjectEntry) this._projectTable[projectItem.Project];
                if (forceRegenerate && (entry != null))
                {
                    entry.RemoveDesignTimeHtml(projectItem);
                }
            }
            else
            {
                this._projectTable = new HybridDictionary();
            }
            if (entry == null)
            {
                entry = new ProjectEntry();
                this._projectTable[projectItem.Project] = entry;
            }
            return entry.GetDesignTimeHtml(projectItem);
        }

        private void OnProjectClosed(object sender, ProjectEventArgs e)
        {
            if (this._projectTable != null)
            {
                this._projectTable.Remove(e.Project);
            }
        }

        private sealed class ProjectEntry
        {
            private MruList _documentList = new MruList(0x10, null, true);
            private IDictionary _documentTable = new HybridDictionary(0x10, true);

            private string GenerateDesignTimeHtml(DocumentProjectItem projectItem)
            {
                string str = null;
                WebFormsDesignerHost designerHost = null;
                try
                {
                    AscxDocument document = new AscxDocument(projectItem);
                    designerHost = new WebFormsDesignerHost(document);
                    document.Load(true);
                    str = string.Empty;
                    string html = document.Html;
                    string directives = document.RegisterDirectives.ToString();
                    string controlText = "<asp:PlaceHolder runat=\"server\">" + html + "</asp:PlaceHolder>";
                    Control control = ControlParser.ParseControl(designerHost, controlText, directives);
                    if ((control == null) || !control.HasControls())
                    {
                        return str;
                    }
                    IDesignerHost host2 = designerHost;
                    IContainer container = host2.Container;
                    StringBuilder builder = new StringBuilder(0x400);
                    bool flag = false;
                    foreach (Control control2 in control.Controls)
                    {
                        if (!(control2 is LiteralControl))
                        {
                            if (!flag)
                            {
                                flag = true;
                                Page component = new Page();
                                component.ID = "Page";
                                container.Add(component, "Page");
                                component.DesignerInitialize();
                            }
                            container.Add(control2);
                        }
                    }
                    foreach (Control control4 in control.Controls)
                    {
                        LiteralControl control5 = control4 as LiteralControl;
                        if (control5 != null)
                        {
                            builder.Append(control5.Text);
                        }
                        else
                        {
                            ControlDesigner designer = (ControlDesigner) host2.GetDesigner(control4);
                            try
                            {
                                string designTimeHtml = designer.GetDesignTimeHtml();
                                builder.Append(designTimeHtml);
                                continue;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    str = builder.ToString();
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (designerHost != null)
                    {
                        ((IDisposable) designerHost).Dispose();
                        designerHost = null;
                    }
                }
                return str;
            }

            public string GetDesignTimeHtml(DocumentProjectItem projectItem)
            {
                string path = projectItem.Path;
                string entry = null;
                string str3 = null;
                str3 = (string) this._documentTable[path];
                if (str3 != null)
                {
                    entry = this._documentList.AddEntry(path);
                }
                else
                {
                    this._documentTable[path] = string.Empty;
                    str3 = this.GenerateDesignTimeHtml(projectItem);
                    if (str3 != null)
                    {
                        this._documentTable[path] = str3;
                        entry = this._documentList.AddEntry(path);
                    }
                    else
                    {
                        this._documentTable.Remove(path);
                    }
                }
                if (entry != null)
                {
                    this._documentList.RemoveEntry(entry);
                }
                return str3;
            }

            public void RemoveDesignTimeHtml(DocumentProjectItem projectItem)
            {
                string path = projectItem.Path;
                if (this._documentTable.Contains(path))
                {
                    this._documentTable.Remove(path);
                    this._documentList.RemoveEntry(path);
                }
            }
        }
    }
}

