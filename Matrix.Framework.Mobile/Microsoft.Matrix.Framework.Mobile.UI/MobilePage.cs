namespace Microsoft.Matrix.Framework.Mobile.UI
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;

    public class MobilePage : System.Web.UI.MobileControls.MobilePage
    {
        public virtual bool isCHTML10(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.get_PreferredRenderingType() == "chtml10");
        }

        public virtual bool isEricssonR380(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Type == "Ericsson R380");
        }

        public virtual bool isGoAmerica(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Browser == "Go.Web");
        }

        public virtual bool isHTML32(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.get_PreferredRenderingType() == "html32");
        }

        public virtual bool isMyPalm(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Browser == "MyPalm");
        }

        public virtual bool isNokia7110(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Type == "Nokia 7110");
        }

        public virtual bool isPocketIE(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Browser == "Pocket IE");
        }

        public virtual bool isUP3X(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Type == "Phone.com 3.x Browser");
        }

        public virtual bool isUP4X(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.Type == "Phone.com 4.x Browser");
        }

        public virtual bool isWML11(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.get_PreferredRenderingType() == "wml11");
        }

        public virtual bool prefersGIF(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.get_PreferredImageMime() == "image/gif");
        }

        public virtual bool prefersWBMP(MobileCapabilities capabilities, string optionalArgument)
        {
            return (capabilities.get_PreferredImageMime() == "image/vnd.wap.wbmp");
        }

        public virtual bool supportsColor(MobileCapabilities capabilities, string optionalArgument)
        {
            return capabilities.get_IsColor();
        }

        public virtual bool supportsCookies(MobileCapabilities capabilities, string optionalArgument)
        {
            return capabilities.Cookies;
        }

        public virtual bool supportsJavaScript(MobileCapabilities capabilities, string optionalArgument)
        {
            return capabilities.JavaScript;
        }

        public virtual bool supportsVoiceCalls(MobileCapabilities capabilities, string optionalArgument)
        {
            return capabilities.get_CanInitiateVoiceCall();
        }
    }
}

