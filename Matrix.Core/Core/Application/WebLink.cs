namespace Microsoft.Matrix.Core.Application
{
    using System;

    public sealed class WebLink
    {
        private string _title;
        private string _url;

        public WebLink(string title, string url)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            if ((url == null) || (url.Length == 0))
            {
                throw new ArgumentNullException("url");
            }
            this._title = title;
            this._url = url;
        }

        public string Title
        {
            get
            {
                return this._title;
            }
        }

        public string Url
        {
            get
            {
                return this._url;
            }
        }
    }
}

