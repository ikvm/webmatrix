namespace Microsoft.Matrix.WebIDE
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    public sealed class Application : MxApplication
    {
        private Application(CommandLine commandLine) : base(commandLine)
        {
        }

        protected override IPackage CreateApplicationPackage()
        {
            return new ApplicationPackage();
        }

        protected override MxApplicationWindow CreateApplicationWindow()
        {
            return new ApplicationWindow(this);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            bool flag = true;
            CommandLine commandLine = new CommandLine(args);
            if (commandLine.Options.Contains("nosplash"))
            {
                flag = false;
            }
            bool flag2 = true;
            if (commandLine.ShowHelp)
            {
                ShowUsage();
                flag2 = false;
            }
            if (flag2)
            {
                if (flag)
                {
                    SplashScreen current = SplashScreen.Current;
                    current.Image = new Bitmap(typeof(Application), "WebIDE.gif");
                    current.Customizer = new SplashScreenCustomizeCallback(Application.SplashScreenCustomizer);
                    current.ShowShadow = true;
                    current.MinimumDuration = 0xbb8;
                    current.Show();
                }
                try
                {
                    new Application(commandLine).Run();
                }
                catch (Exception)
                { }
            }
        }

        protected override bool OnUnhandledException(Exception e)
        {
            if (e is NullReferenceException)
            {
                MethodBase targetSite = e.TargetSite;
                if (targetSite.Name.Equals("OnCustomDraw") && targetSite.DeclaringType.Name.Equals("PageSelector"))
                {
                    return true;
                }
            }
            return base.OnUnhandledException(e);
        }

        private static void ShowUsage()
        {
        }

        private static void SplashScreenCustomizer(SplashScreenSurface surface)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(typeof(Application).Module.FullyQualifiedName);
            string s = string.Format("Version {0}.{1} (Build {2}) (Technology Preview)", versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart);
            string str2 = ".NET Framework Version " + Environment.Version;
            Graphics graphics = surface.Graphics;
            Pen pen = new Pen(Color.Black, 2f);
            Font font = new Font("Tahoma", 8f);
            graphics.DrawRectangle(pen, 1, 1, surface.Bounds.Width - 2, surface.Bounds.Height - 2);
            graphics.DrawString(s, font, Brushes.Black, (float) 117f, (float) 250f);
            graphics.DrawString(str2, font, Brushes.Black, (float) 117f, (float) 266f);
            pen.Dispose();
            font.Dispose();
        }

        protected override ApplicationType ApplicationType
        {
            get
            {
                return ApplicationType.Workspace;
            }
        }

        protected override string ComponentsPath
        {
            get
            {
                return Path.Combine(base.ApplicationPath, "Components");
            }
        }

        protected override string HelpUrl
        {
            get
            {
                return Path.Combine(Path.Combine(base.ApplicationPrivatePath, "Help"), "StartHere.htm");
            }
        }

        protected override string PluginsPath
        {
            get
            {
                return Path.Combine(base.ApplicationPath, "Plugins");
            }
        }

        protected override string PreferencesFileName
        {
            get
            {
                return "WebMatrix.settings";
            }
        }

        //protected override string TemplatesPath
        public override string TemplatesPath
        {
            get
            {
                return Path.Combine(base.ApplicationPrivatePath, "Templates");
            }
        }

        protected override string Title
        {
            get
            {
                return "Microsoft ASP.NET Web Matrix";
            }
        }
    }
}

