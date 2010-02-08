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
    class User : INotifyPropertyChanged
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
        private string m_UserName;
        public string UserName
        {
            get{ return m_UserName;}
            set{ m_UserName = value; OnPropertyChanged(new PropertyChangedEventArgs("UserName"));}
        }
        private string m_Password;
        public string Password
        {
            get{ return m_Password;}
            set{ m_Password = value; OnPropertyChanged(new PropertyChangedEventArgs("Password"));}
        }
        private string m_LastLogin;
        public string LastLogin
        {
            get{ return m_LastLogin;}
            set{ m_LastLogin = value; OnPropertyChanged(new PropertyChangedEventArgs("LastLogin"));}
        }
    }
}
