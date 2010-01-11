namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.DBAdmin.Documents;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using Microsoft.Matrix.UIComponents;
    using System;

    public class StoredProcedureSourceView : SourceView
    {
        public StoredProcedureSourceView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override bool HandleCommand(Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 4))
            {
                flag = true;
            }
            if (!flag)
            {
                flag = base.HandleCommand(command);
            }
            return flag;
        }

        protected override bool SaveToDocument()
        {
            DatabaseProject project = (DatabaseProject) base.Document.ProjectItem.Project;
            try
            {
                project.Database.StoredProcedures[((StoredProcedureDocument) base.Document).Name].Update(this.Text);
            }
            catch (Exception exception)
            {
                ((IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService))).ReportError(exception, "There was an error saving the stored procedure.", false);
                return false;
            }
            return true;
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 4))
            {
                command.Enabled = false;
                flag = true;
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }
    }
}

