namespace Microsoft.Matrix.Packages.Community.UserInterface
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Packages.Community;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Drawing;

    public sealed class BuddyCommunityTab : CommunityTab
    {
        private string _buddyGroup;
        private Interop.IMessenger2 _messenger;
        private bool _signingIn;
        private static Image awayBuddyGlyph;
        private static Image buddyGlyph;
        private static Image busyBuddyGlyph;
        private static Image errorGlyph;
        private static Image infoGlyph;
        private const string MessengerCommand = "Messenger";
        private static Image offineBuddyGlyph;
        private const string SignInCommand = "SignIn";
        private static Image watermarkImage;

        protected override void Dispose()
        {
            this._messenger = null;
            base.Dispose();
        }

        private void GenerateContactsDisplayPage(CommunityTab.CommunityPageBuilder builder)
        {
            Interop.IMessengerGroup group = null;
            int num = 0;
            if (this._messenger != null)
            {
                Interop.IMessengerGroups myGroups = (Interop.IMessengerGroups) this._messenger.GetMyGroups();
                int count = myGroups.GetCount();
                for (int i = 0; i < count; i++)
                {
                    Interop.IMessengerGroup group2 = (Interop.IMessengerGroup) myGroups.Item(i);
                    string name = group2.GetName();
                    if ((name != null) && name.Equals(this._buddyGroup))
                    {
                        group = group2;
                    }
                }
                if (group == null)
                {
                    group = (Interop.IMessengerGroup) this._messenger.CreateGroup(this._buddyGroup, this._messenger.GetMyServiceId());
                }
            }
            if (group != null)
            {
                Interop.IMessengerContacts contacts = (Interop.IMessengerContacts) group.GetContacts();
                if (contacts != null)
                {
                    num = contacts.GetCount();
                    for (int j = 0; j < num; j++)
                    {
                        string str2;
                        Interop.IMessengerContact contact = (Interop.IMessengerContact) contacts.Item(j);
                        if (contact.GetBlocked())
                        {
                            goto Label_0165;
                        }
                        Image glyph = null;
                        int status = contact.GetStatus();
                        if (status <= 10)
                        {
                            switch (status)
                            {
                                case 1:
                                    goto Label_0121;

                                case 2:
                                    glyph = BuddyGlyph;
                                    goto Label_0128;

                                case 10:
                                    goto Label_0118;
                            }
                            goto Label_0121;
                        }
                        if ((status != 0x12) && (status != 0x22))
                        {
                            goto Label_0121;
                        }
                        glyph = AwayBuddyGlyph;
                        goto Label_0128;
                    Label_0118:
                        glyph = BusyBuddyGlyph;
                        goto Label_0128;
                    Label_0121:
                        glyph = OfflineBuddyGlyph;
                    Label_0128:
                        str2 = contact.GetSigninName();
                        string serviceId = contact.GetServiceId();
                        string friendlyName = contact.GetFriendlyName();
                        builder.AddHyperLinkWithGlyph(friendlyName, new string[] { str2, serviceId }, str2, glyph);
                    Label_0165:;
                    }
                }
            }
            if (num == 0)
            {
                builder.BeginNewSection(false);
                builder.AddImage(InfoGlyph).Padding = new BoxEdges(0, 1, 4, 1);
                builder.PushBold();
                builder.AddText("No contacts found...");
                builder.PopBold();
                builder.EndCurrentSection();
                builder.AddSectionBreak();
                builder.BeginNewSection(20);
                builder.AddTextSpan("There are currently no contacts present in your '" + this._buddyGroup + "' group in Windows Messenger to display here.");
                builder.EndCurrentSection();
            }
            else
            {
                builder.AddSectionBreak();
                builder.AddSectionBreak();
                builder.AddHorizontalLine();
                builder.BeginNewSection(false);
                builder.AddImage(InfoGlyph).Padding = new BoxEdges(0, 1, 4, 1);
                builder.PushBold();
                builder.AddText("Organizing your contacts...");
                builder.PopBold();
                builder.EndCurrentSection();
            }
            builder.AddSectionBreak();
            builder.BeginNewSection(20);
            builder.AddTextSpan("You can use Windows Messenger to organize your contact list, so that you have other contacts appear in this group.");
            builder.EndCurrentSection();
        }

        protected override void GenerateDisplayPage(CommunityTab.CommunityPageBuilder builder)
        {
            base.GenerateDisplayPage(builder);
            builder.SetPageWatermark(new Watermark(WatermarkImage, WatermarkPlacement.BottomRight));
            if (this._messenger == null)
            {
                try
                {
                    Interop.Messenger messenger = new Interop.Messenger();
                    this._messenger = (Interop.IMessenger2) messenger;
                }
                catch
                {
                }
            }
            builder.AddHeading(this._buddyGroup, null);
            builder.AddHorizontalLine();
            if (this._messenger == null)
            {
                this.GenerateErrorDisplayPage(builder);
            }
            else
            {
                try
                {
                    int myStatus = this._messenger.GetMyStatus();
                    if ((myStatus == 0) || (myStatus == 1))
                    {
                        this.GenerateSignInDisplayPage(builder);
                    }
                    else
                    {
                        this.GenerateContactsDisplayPage(builder);
                        this._signingIn = false;
                    }
                }
                catch
                {
                    if (this._signingIn)
                    {
                        this.GenerateSigningInDisplayPage(builder);
                    }
                    else
                    {
                        this.GenerateErrorDisplayPage(builder);
                    }
                }
            }
        }

        private void GenerateErrorDisplayPage(CommunityTab.CommunityPageBuilder builder)
        {
            builder.BeginNewSection(false);
            builder.AddImage(ErrorGlyph).Padding = new BoxEdges(0, 1, 4, 1);
            builder.PushBold();
            builder.AddText("Error...");
            builder.PopBold();
            builder.EndCurrentSection();
            builder.AddSectionBreak();
            builder.BeginNewSection(20);
            builder.AddTextSpan("Unable to access the Windows Messenger service and display contacts in the '" + this._buddyGroup + "' contact group. To use the contact list feature you must have Windows Messenger version 4.5 or greater on your computer.");
            builder.EndCurrentSection();
            builder.AddSectionBreak();
            builder.BeginNewSection(20);
            builder.AddHyperLink("Click here", "Messenger").ToolTip = "http://messenger.msn.com";
            builder.AddTextSpan(" to download and install the latest version of Windows Messenger. Once you have installed it, click the 'Refresh' button above.");
            builder.EndCurrentSection();
        }

        private void GenerateSignInDisplayPage(CommunityTab.CommunityPageBuilder builder)
        {
            builder.BeginNewSection(false);
            builder.AddImage(InfoGlyph).Padding = new BoxEdges(0, 1, 4, 1);
            builder.PushBold();
            builder.AddText("Please sign-in...");
            builder.PopBold();
            builder.EndCurrentSection();
            builder.AddSectionBreak();
            builder.BeginNewSection(20);
            builder.AddTextSpan("You can use the contact list feature to interact with your contacts in the '" + this._buddyGroup + "' group by signing into Windows Messenger service.");
            builder.EndCurrentSection();
            builder.AddSectionBreak();
            builder.BeginNewSection(20);
            builder.AddHyperLink("Sign-in", "SignIn").ToolTip = "Click here to sign-in now";
            builder.EndCurrentSection();
        }

        private void GenerateSigningInDisplayPage(CommunityTab.CommunityPageBuilder builder)
        {
            builder.BeginNewSection(false);
            builder.AddImage(InfoGlyph).Padding = new BoxEdges(0, 1, 4, 1);
            builder.PushBold();
            builder.AddText("Signing-in...");
            builder.PopBold();
            builder.EndCurrentSection();
        }

        public override void Initialize(IServiceProvider serviceProvider, string initializationData)
        {
            base.Initialize(serviceProvider, initializationData);
            if ((initializationData == null) || (initializationData.Length == 0))
            {
                this._buddyGroup = "My Contacts";
            }
            else
            {
                this._buddyGroup = initializationData;
            }
        }

        protected override void OnLinkClicked(object userData)
        {
            if (userData is string)
            {
                if (!userData.Equals("SignIn"))
                {
                    if (userData.Equals("Messenger"))
                    {
                        IWebBrowsingService service = (IWebBrowsingService) base.ServiceProvider.GetService(typeof(IWebBrowsingService));
                        if (service != null)
                        {
                            service.BrowseUrl("http://messenger.msn.com");
                        }
                    }
                }
                else
                {
                    try
                    {
                        this._signingIn = true;
                        int num = this._messenger.AutoSignin();
                        if (((num != -2130705660) && (num != 0)) && (num != 1))
                        {
                            this._messenger.SignIn(IntPtr.Zero, null, null);
                        }
                        this.OnDisplayChanged(EventArgs.Empty);
                    }
                    catch
                    {
                        this._signingIn = false;
                    }
                }
            }
            else
            {
                string[] strArray = userData as string[];
                if (this._messenger != null)
                {
                    try
                    {
                        string bstrSigninName = strArray[0];
                        string bstrServiceId = strArray[1];
                        Interop.IMessengerContact vContact = (Interop.IMessengerContact) this._messenger.GetContact(bstrSigninName, bstrServiceId);
                        if (vContact != null)
                        {
                            this._messenger.InstantMessage(vContact);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static Image AwayBuddyGlyph
        {
            get
            {
                if (awayBuddyGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(BuddyCommunityTab), "AwayBuddy.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    awayBuddyGlyph = bitmap;
                }
                return awayBuddyGlyph;
            }
        }

        private static Image BuddyGlyph
        {
            get
            {
                if (buddyGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(BuddyCommunityTab), "Buddy.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    buddyGlyph = bitmap;
                }
                return buddyGlyph;
            }
        }

        private static Image BusyBuddyGlyph
        {
            get
            {
                if (busyBuddyGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(BuddyCommunityTab), "BusyBuddy.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    busyBuddyGlyph = bitmap;
                }
                return busyBuddyGlyph;
            }
        }

        private static Image ErrorGlyph
        {
            get
            {
                if (errorGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(BuddyCommunityTab), "ErrorGlyph.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    errorGlyph = bitmap;
                }
                return errorGlyph;
            }
        }

        public override Image Glyph
        {
            get
            {
                return BuddyGlyph;
            }
        }

        private static Image InfoGlyph
        {
            get
            {
                if (infoGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(BuddyCommunityTab), "InfoGlyph.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    infoGlyph = bitmap;
                }
                return infoGlyph;
            }
        }

        public override string Name
        {
            get
            {
                return "Contacts";
            }
        }

        private static Image OfflineBuddyGlyph
        {
            get
            {
                if (offineBuddyGlyph == null)
                {
                    Bitmap bitmap = new Bitmap(typeof(BuddyCommunityTab), "OfflineBuddy.bmp");
                    bitmap.MakeTransparent(Color.Fuchsia);
                    offineBuddyGlyph = bitmap;
                }
                return offineBuddyGlyph;
            }
        }

        public override int RefreshRate
        {
            get
            {
                if (this._messenger != null)
                {
                    return 0x2710;
                }
                return 0;
            }
        }

        public override bool SupportsRefresh
        {
            get
            {
                return true;
            }
        }

        private static Image WatermarkImage
        {
            get
            {
                if (watermarkImage == null)
                {
                    watermarkImage = new Bitmap(typeof(BuddyCommunityTab), "BuddyBackground.jpg");
                }
                return watermarkImage;
            }
        }
    }
}

