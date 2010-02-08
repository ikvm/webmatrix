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
    class Effect_I : INotifyPropertyChanged
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
            get{ return m_ID;}
            set{ m_ID = value; OnPropertyChanged(new PropertyChangedEventArgs("ID"));}
        }
        private string m_Desc;
        public string Desc
        {
            get{ return m_Desc;}
            set{ m_Desc = value; OnPropertyChanged(new PropertyChangedEventArgs("Desc"));}
        }
        private int m_Interval;
        public int Interval
        {
            get{ return m_Interval;}
            set{ m_Interval = value; OnPropertyChanged(new PropertyChangedEventArgs("Interval"));}
        }
        private int m_LaunchType;
        public int LaunchType
        {
            get{ return m_LaunchType;}
            set{ m_LaunchType = value; OnPropertyChanged(new PropertyChangedEventArgs("LaunchType"));}
        }
        private string m_Precond;
        public string Precond
        {
            get{ return m_Precond;}
            set{ m_Precond = value; OnPropertyChanged(new PropertyChangedEventArgs("Precond"));}
        }
        private string m_OPIn;
        public string OPIn
        {
            get{ return m_OPIn;}
            set{ m_OPIn = value; OnPropertyChanged(new PropertyChangedEventArgs("OPIn"));}
        }
        private string m_OPOut;
        public string OPOut
        {
            get{ return m_OPOut;}
            set{ m_OPOut = value; OnPropertyChanged(new PropertyChangedEventArgs("OPOut"));}
        }
    }
}
