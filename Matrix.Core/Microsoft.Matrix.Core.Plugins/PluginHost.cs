namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.UIComponents;
    using System;

    internal abstract class PluginHost : IServiceProvider
    {
        private IServiceProvider _serviceProvider;

        public PluginHost(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected object RunPlugin(string pluginTypeName)
        {
            return this.RunPlugin(pluginTypeName, null);
        }

        protected object RunPlugin(string pluginTypeName, object initializationData)
        {
            string format = null;
            object obj2 = null;
            Type c = Type.GetType(pluginTypeName, false);
            if ((c != null) && this.PluginBaseType.IsAssignableFrom(c))
            {
                Plugin plugin = null;
                try
                {
                    plugin = (Plugin) Activator.CreateInstance(c);
                    obj2 = plugin.RunPlugin(this, initializationData);
                    goto Label_0070;
                }
                catch (Exception exception)
                {
                    if (plugin != null)
                    {
                        format = "There was an error running the selected {0}.\r\n" + exception.Message;
                    }
                    else
                    {
                        format = "There was an error instantiating the selected {0}.";
                    }
                    goto Label_0070;
                }
                finally
                {
                    if (plugin != null)
                    {
                        try
                        {
                            ((IDisposable) plugin).Dispose();
                        }
                        catch
                        {
                        }
                        plugin = null;
                    }
                }
            }
            format = "Unable to load the selected {0}.";
        Label_0070:
            if (format != null)
            {
                format = string.Format(format, this.PluginBaseType.Name);
                IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                if (service != null)
                {
                    service.ReportError(format, this.Description, false);
                }
            }
            return obj2;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (this._serviceProvider != null)
            {
                return this._serviceProvider.GetService(serviceType);
            }
            return null;
        }

        protected abstract string Description { get; }

        protected abstract Type PluginBaseType { get; }
    }
}

