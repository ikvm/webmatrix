namespace Microsoft.Matrix.Core.Services
{
    using System;
    using System.Diagnostics;

    internal sealed class WebBrowsingService : IWebBrowsingService, IDisposable
    {
        private IServiceProvider _serviceProvider;

        public WebBrowsingService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private bool BrowseUrlWithShellWebBrowser(string url)
        {
            if ((url == null) || (url.Length == 0))
            {
                return false;
            }
            Process process = new Process();
            process.StartInfo.FileName = url;
            process.StartInfo.Verb = "open";
            process.StartInfo.UseShellExecute = true;
            bool flag = false;
            try
            {
                flag = process.Start();
            }
            catch (Exception)
            {
            }
            return flag;
        }

        bool IWebBrowsingService.BrowseUrl(string url)
        {
            return this.BrowseUrlWithShellWebBrowser(url);
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }
    }
}

