using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using DateExplorer.Core.Controllers;
using System.ComponentModel;
using DataExplorer.Core.Models;

namespace DateExplorer.Core.Models
{
    class Buff : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private int m_ID;
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; OnPropertyChanged(new PropertyChangedEventArgs("ID")); }
        }
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; OnPropertyChanged(new PropertyChangedEventArgs("Name")); }
        }
        private string m_Desc;
        public string Desc
        {
            get { return m_Desc; }
            set { m_Desc = value; OnPropertyChanged(new PropertyChangedEventArgs("Desc")); }
        }
        private int m_Persist;
        public int Persist
        {
            get { return m_Persist; }
            set { m_Persist = value; OnPropertyChanged(new PropertyChangedEventArgs("Persist")); }
        }
        private int m_Group;
        public int Group
        {
            get { return m_Group; }
            set { m_Group = value; OnPropertyChanged(new PropertyChangedEventArgs("Group")); }
        }
        private int m_Level;
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; OnPropertyChanged(new PropertyChangedEventArgs("Level")); }
        }
        private string m_Category;
        public string Category
        {
            get { return m_Category; }
            set { m_Category = value; OnPropertyChanged(new PropertyChangedEventArgs("Category")); }
        }
        private string m_Category1;
        public string Category1
        {
            get { return m_Category1; }
            set { m_Category1 = value; OnPropertyChanged(new PropertyChangedEventArgs("Category1")); }
        }
        private string m_Category2;
        public string Category2
        {
            get { return m_Category2; }
            set { m_Category2 = value; OnPropertyChanged(new PropertyChangedEventArgs("Category2")); }
        }
        private string m_Category3;
        public string Category3
        {
            get { return m_Category3; }
            set { m_Category3 = value; OnPropertyChanged(new PropertyChangedEventArgs("Category3")); }
        }
        private int m_Capability;
        public int Capability
        {
            get { return m_Capability; }
            set { m_Capability = value; OnPropertyChanged(new PropertyChangedEventArgs("Capability")); }
        }
        private int m_Stack;
        public int Stack
        {
            get { return m_Stack; }
            set { m_Stack = value; OnPropertyChanged(new PropertyChangedEventArgs("Stack")); }
        }
        private string m_ErrProc;
        public string ErrProc
        {
            get { return m_ErrProc; }
            set { m_ErrProc = value; OnPropertyChanged(new PropertyChangedEventArgs("ErrProc")); }
        }
        private string m_Precond;
        public string Precond
        {
            get { return m_Precond; }
            set { m_Precond = value; OnPropertyChanged(new PropertyChangedEventArgs("Precond")); }
        }
        private int m_SpecialStack;
        public int SpecialStack
        {
            get { return m_SpecialStack; }
            set { m_SpecialStack = value; OnPropertyChanged(new PropertyChangedEventArgs("SpecialStack")); }
        }
        private int m_Sync;
        public int Sync
        {
            get { return m_Sync; }
            set { m_Sync = value; OnPropertyChanged(new PropertyChangedEventArgs("Sync")); }
        }
        private int m_Save;
        public int Save
        {
            get { return m_Save; }
            set { m_Save = value; OnPropertyChanged(new PropertyChangedEventArgs("Save")); }
        }
        private int m_Delay;
        public int Delay
        {
            get { return m_Delay; }
            set { m_Delay = value; OnPropertyChanged(new PropertyChangedEventArgs("Delay")); }
        }
        private int m_Plus;
        public int Plus
        {
            get { return m_Plus; }
            set { m_Plus = value; OnPropertyChanged(new PropertyChangedEventArgs("Plus")); }
        }
        private int m_Send;
        public int Send
        {
            get { return m_Send; }
            set { m_Send = value; OnPropertyChanged(new PropertyChangedEventArgs("Send")); }
        }
        private int m_Death;
        public int Death
        {
            get { return m_Death; }
            set { m_Death = value; OnPropertyChanged(new PropertyChangedEventArgs("Death")); }
        }
        private string m_Offline;
        public string Offline
        {
            get { return m_Offline; }
            set { m_Offline = value; OnPropertyChanged(new PropertyChangedEventArgs("Offline")); }
        }
        private string m_DelayAdd;
        public string DelayAdd
        {
            get { return m_DelayAdd; }
            set { m_DelayAdd = value; OnPropertyChanged(new PropertyChangedEventArgs("DelayAdd")); }
        }
        private string m_DelayAddType;
        public string DelayAddType
        {
            get { return m_DelayAddType; }
            set { m_DelayAddType = value; OnPropertyChanged(new PropertyChangedEventArgs("DelayAddType")); }
        }
        private string m_Icon;
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; OnPropertyChanged(new PropertyChangedEventArgs("Icon")); }
        }
        private string m_ImageSet;
        public string ImageSet
        {
            get { return m_ImageSet; }
            set { m_ImageSet = value; OnPropertyChanged(new PropertyChangedEventArgs("ImageSet")); }
        }
        private ImageInfo m_ReplaceRes;
        public ImageInfo ReplaceRes
        {
            get { return m_ReplaceRes; }
            set { m_ReplaceRes = value; OnPropertyChanged(new PropertyChangedEventArgs("ReplaceRes")); }
        }
        private ImageInfo m_OnceRes;
        public ImageInfo OnceRes
        {
            get { return m_OnceRes; }
            set { m_OnceRes = value; OnPropertyChanged(new PropertyChangedEventArgs("OnceRes")); }
        }
        private ImageInfo m_HeadRes;
        public ImageInfo HeadRes
        {
            get { return m_HeadRes; }
            set { m_HeadRes = value; OnPropertyChanged(new PropertyChangedEventArgs("HeadRes")); }
        }
        private ImageInfo m_BodyRes;
        public ImageInfo BodyRes
        {
            get { return m_BodyRes; }
            set { m_BodyRes = value; OnPropertyChanged(new PropertyChangedEventArgs("BodyRes")); }
        }
        private ImageInfo m_BottomRes;
        public ImageInfo BottomRes
        {
            get { return m_BottomRes; }
            set { m_BottomRes = value; OnPropertyChanged(new PropertyChangedEventArgs("BottomRes")); }
        }
        private ImageInfo m_OnlySoundRes;
        public ImageInfo OnlySoundRes
        {
            get { return m_OnlySoundRes; }
            set { m_OnlySoundRes = value; OnPropertyChanged(new PropertyChangedEventArgs("OnlySoundRes")); }
        }
        private ImageInfo m_SoundRes;
        public ImageInfo SoundRes
        {
            get { return m_SoundRes; }
            set { m_SoundRes = value; OnPropertyChanged(new PropertyChangedEventArgs("SoundRes")); }
        }
        private string m_Ie1;
        public string Ie1
        {
            get { return m_Ie1; }
            set { m_Ie1 = value; OnPropertyChanged(new PropertyChangedEventArgs("Ie1")); }
        }
        private string m_Ie2;
        public string Ie2
        {
            get { return m_Ie2; }
            set { m_Ie2 = value; OnPropertyChanged(new PropertyChangedEventArgs("Ie2")); }
        }
        private string m_Ie3;
        public string Ie3
        {
            get { return m_Ie3; }
            set { m_Ie3 = value; OnPropertyChanged(new PropertyChangedEventArgs("Ie3")); }
        }
        private string m_Ie4;
        public string Ie4
        {
            get { return m_Ie4; }
            set { m_Ie4 = value; OnPropertyChanged(new PropertyChangedEventArgs("Ie4")); }
        }
        private string m_Ie5;
        public string Ie5
        {
            get { return m_Ie5; }
            set { m_Ie5 = value; OnPropertyChanged(new PropertyChangedEventArgs("Ie5")); }
        }
        private string m_Pe1;
        public string Pe1
        {
            get { return m_Pe1; }
            set { m_Pe1 = value; OnPropertyChanged(new PropertyChangedEventArgs("Pe1")); }
        }
        private string m_Pe2;
        public string Pe2
        {
            get { return m_Pe2; }
            set { m_Pe2 = value; OnPropertyChanged(new PropertyChangedEventArgs("Pe2")); }
        }
        private string m_Pe3;
        public string Pe3
        {
            get { return m_Pe3; }
            set { m_Pe3 = value; OnPropertyChanged(new PropertyChangedEventArgs("Pe3")); }
        }
        private string m_Pe4;
        public string Pe4
        {
            get { return m_Pe4; }
            set { m_Pe4 = value; OnPropertyChanged(new PropertyChangedEventArgs("Pe4")); }
        }
        private string m_Pe5;
        public string Pe5
        {
            get { return m_Pe5; }
            set { m_Pe5 = value; OnPropertyChanged(new PropertyChangedEventArgs("Pe5")); }
        }
        private string m_TriggerMask;
        public string TriggerMask
        {
            get { return m_TriggerMask; }
            set { m_TriggerMask = value; OnPropertyChanged(new PropertyChangedEventArgs("TriggerMask")); }
        }
        private string m_Scale;
        public string Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; OnPropertyChanged(new PropertyChangedEventArgs("Scale")); }
        }
        private string m_Alpha;
        public string Alpha
        {
            get { return m_Alpha; }
            set { m_Alpha = value; OnPropertyChanged(new PropertyChangedEventArgs("Alpha")); }
        }
        private string m_Color;
        public string Color
        {
            get { return m_Color; }
            set { m_Color = value; OnPropertyChanged(new PropertyChangedEventArgs("Color")); }
        }
    }
}
