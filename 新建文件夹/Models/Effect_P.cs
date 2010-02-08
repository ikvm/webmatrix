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
    class Effect_P : INotifyPropertyChanged
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
        private string m_Event;
        public string Event
        {
            get{ return m_Event;}
            set{ m_Event = value; OnPropertyChanged(new PropertyChangedEventArgs("Event"));}
        }
        private string m_OutAction;
        public string OutAction
        {
            get{ return m_OutAction;}
            set{ m_OutAction = value; OnPropertyChanged(new PropertyChangedEventArgs("OutAction"));}
        }
        private int m_Percent;
        public int Percent
        {
            get{ return m_Percent;}
            set{ m_Percent = value; OnPropertyChanged(new PropertyChangedEventArgs("Percent"));}
        }
        private string m_Condition;
        public string Condition
        {
            get{ return m_Condition;}
            set{ m_Condition = value; OnPropertyChanged(new PropertyChangedEventArgs("Condition"));}
        }
        private string m_EventOP;
        public string EventOP
        {
            get{ return m_EventOP;}
            set{ m_EventOP = value; OnPropertyChanged(new PropertyChangedEventArgs("EventOP"));}
        }
    }
}
