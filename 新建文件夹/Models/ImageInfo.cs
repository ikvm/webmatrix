using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DataExplorer;
using System.Windows;
using System.Collections;
using FreeImageAPI;

namespace DataExplorer.Core.Models
{
    public class ImageInfo : ModelBase
    {
        private string m_ImageSetName;
        public string ImageSetName
        {
            get { return m_ImageSetName; }
            set { m_ImageSetName = value; }
        }
        private string m_ImageName;
        public string ImageName
        {
            get { return m_ImageName; }
            set { m_ImageName = value; }
        }
        private Image m_Image;
        public Image Image
        {
            get 
            { 
                if(m_Image == null)
                    LoadImage();

                return m_Image; 
            }

            set { m_Image = value; }
        }

        public ImageInfo(string imageSetName, string imageName)
        {
            m_ImageSetName = imageSetName;
            m_ImageName = imageName;

        }

        private void LoadImage()
        {
            ImageSetCollection sets = Globals.Instance["ImageSets"] as ImageSetCollection;
            ImageSet set = sets[this.m_ImageSetName];
            if (set == null)
                return;

            Rectangle rect = set[this.m_ImageName];
            if (rect == null)
                return;

            FREE_IMAGE_LOAD_FLAGS flag;
            flag = set.ImageFileName.ToLower().EndsWith(".tga") ? FREE_IMAGE_LOAD_FLAGS.TARGA_LOAD_RGB888 : FREE_IMAGE_LOAD_FLAGS.JPEG_FAST;
            string imageFile = string.Format("{0}\\{1}\\{2}", Globals.Instance["CvsPath"], @"fscraft\client", set.ImageFileName);
            FIBITMAP dib = FreeImage.LoadEx(imageFile, flag);
            if(!dib.IsNull)
            {
                FIBITMAP copy = FreeImage.Copy(dib, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                if (!copy.IsNull)
                { 
                    this.m_Image = FreeImage.GetBitmap(copy);

                    FreeImage.UnloadEx(ref copy);
                }
                
                FreeImage.UnloadEx(ref dib);
            }
        }

    }
}
