namespace Microsoft.Matrix.Packages.Community.UserInterface
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Services;
    using System;
    using System.Collections;
    using System.Drawing;

    public class LinksCommunityTab : CommunityTab
    {
        private string[] _headings;
        private string _name;
        private string[] _sectionNames;
        private static Image linkImage;

        protected override void GenerateDisplayPage(CommunityTab.CommunityPageBuilder builder)
        {
            base.GenerateDisplayPage(builder);
            IApplicationIdentity service = (IApplicationIdentity) base.ServiceProvider.GetService(typeof(IApplicationIdentity));
            if (service != null)
            {
                IDictionary webLinks = service.WebLinks;
                if ((webLinks != null) && (webLinks.Count != 0))
                {
                    for (int i = 0; i < this._headings.Length; i++)
                    {
                        string str = this._sectionNames[i];
                        if ((str != null) && (str.Length != 0))
                        {
                            if (i != 0)
                            {
                                builder.AddSectionBreak();
                            }
                            WebLink link = (WebLink) webLinks[str + "0"];
                            if (link != null)
                            {
                                string text1 = this._headings[i];
                                builder.AddHeading(this._headings[i], null);
                                builder.AddHorizontalLine();
                                int num2 = 0;
                                while (link != null)
                                {
                                    builder.AddHyperLinkWithGlyph(link.Title, link.Url, null, LinkImage);
                                    num2++;
                                    link = (WebLink) webLinks[str + num2];
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Initialize(IServiceProvider serviceProvider, string initializationData)
        {
            base.Initialize(serviceProvider, initializationData);
            bool flag = false;
            string[] strArray = initializationData.Split(new char[] { '|' });
            if (strArray.Length < 2)
            {
                flag = true;
            }
            else
            {
                this._name = strArray[0];
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                for (int i = 1; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { ';' });
                    if (strArray2.Length != 2)
                    {
                        break;
                    }
                    list.Add(strArray2[0]);
                    list2.Add(strArray2[1]);
                }
                if (list.Count != (strArray.Length - 1))
                {
                    flag = true;
                }
                else
                {
                    this._headings = (string[]) list.ToArray(typeof(string));
                    this._sectionNames = (string[]) list2.ToArray(typeof(string));
                }
            }
            if (flag)
            {
                throw new ArgumentException("initializationData");
            }
        }

        protected override void OnLinkClicked(object userData)
        {
            string url = userData as string;
            IWebBrowsingService service = (IWebBrowsingService) base.ServiceProvider.GetService(typeof(IWebBrowsingService));
            if (service != null)
            {
                service.BrowseUrl(url);
            }
        }

        public override Image Glyph
        {
            get
            {
                Bitmap bitmap = new Bitmap(typeof(LinksCommunityTab), "Links.bmp");
                bitmap.MakeTransparent(Color.Fuchsia);
                return bitmap;
            }
        }

        private static Image LinkImage
        {
            get
            {
                if (linkImage == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(LinksCommunityTab), "LinkGlyph.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    linkImage = bitmap;
                }
                return linkImage;
            }
        }

        public override string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

