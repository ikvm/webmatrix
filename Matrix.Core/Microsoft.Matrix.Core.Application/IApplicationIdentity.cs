namespace Microsoft.Matrix.Core.Application
{
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public interface IApplicationIdentity
    {
        event EventHandler Startup;

        string GetSetting(string settingName);
        string GetSetting(string settingName, bool allowCommandLineOverride);
        bool OnUnhandledException(Exception e);
        void SetSetting(string settingName, string settingValue);
        void ShowHelpTopics();

        string ApplicationPath { get; }

        string ApplicationPrivatePath { get; }

        Microsoft.Matrix.Core.Application.ApplicationType ApplicationType { get; }

        MxApplicationWindow ApplicationWindow { get; }

        Microsoft.Matrix.Utility.CommandLine CommandLine { get; }

        string ComponentsPath { get; }

        string Name { get; }

        string PluginsPath { get; }

        string PreferencesFileName { get; }

        string TemplatesPath { get; }

        string Title { get; }

        IDictionary WebLinks { get; }
    }
}

