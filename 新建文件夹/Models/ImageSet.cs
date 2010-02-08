using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace DataExplorer.Core.Models
{
    public class ImageSet:Dictionary<string, Rectangle>
    {
        private string m_ImageSetName;
        public string ImageSetName
        {
            get { return m_ImageSetName; }
            set { m_ImageSetName = value; }
        }
        private string m_ImageFileName;
        public string ImageFileName
        {
            get { return m_ImageFileName; }
            set { m_ImageFileName = value; }
        }

        public ImageSet(string imageSetName)
        {
            m_ImageSetName = imageSetName;
            LoadImageSet();
        }

        private void LoadImageSet()
        {
            string cvsPath = Globals.Instance["CvsPath"].ToString();
            string imageSetPath = string.Format(@"{0}\{1}\{2}.ims", cvsPath, @"fscraft\client\uisettings\imagesets\dotaui", m_ImageSetName);

            string content = File.ReadAllText(imageSetPath);
            Match imageFileMatch = Regex.Match(content, @"\<Imageset\s*[^\s]*?\s*Imagefile=(?<ImageFile>[^ \>]+)", RegexOptions.IgnoreCase);
            if (!imageFileMatch.Success)
                return;

            string imageFile = imageFileMatch.Groups[1].Value;
            m_ImageFileName = imageFile.Trim(new char[] { '"' });

            string pattern = @"\<Image\s*Name=(?<Name>[^\s]*)\s*XPos=(?<X>[""\d]*)\s*YPos=(?<Y>[""\d]*)\s*Width=(?<Width>[""\d]*)\s*Height=(?<Height>[""\d]*).*";
            MatchCollection matches = Regex.Matches(content, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            { 
                if(match.Success)
                {
                    string name = match.Groups["Name"].Value.Trim(new char[] { '"' });
                    string xPos = match.Groups["X"].Value.Trim(new char[] { '"' });
                    string yPos = match.Groups["Y"].Value.Trim(new char[] { '"' });
                    string width = match.Groups["Width"].Value.Trim(new char[] { '"' });
                    string height = match.Groups["Height"].Value.Trim(new char[] { '"' });

                    Rectangle rect = new Rectangle();
                    rect.X = int.Parse(xPos);
                    rect.Y = int.Parse(yPos);
                    rect.Width = int.Parse(width);
                    rect.Height = int.Parse(height);

                    this[name] = rect;
               }
            }
            /*
            XmlReaderSettings settings = new XmlReaderSettings();
            
            XmlDocument doc = new XmlDocument();
            doc.Load(imageSetPath);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                Rectangle rect = new Rectangle();
                rect.X = int.Parse(node.Attributes["XPos"].Value);
                rect.Y = int.Parse(node.Attributes["YPos"].Value);
                rect.Width = int.Parse(node.Attributes["Width"].Value);
                rect.Height = int.Parse(node.Attributes["Height"].Value);

                this[node.Attributes["Name"].Value] = rect;
            }*/
        }

    }
}
