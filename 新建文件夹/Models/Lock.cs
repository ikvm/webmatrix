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
    class Lock : INotifyPropertyChanged
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
        private int m_LockerID;
        public int LockerID
        {
            get{ return m_LockerID;}
            set{ m_LockerID = value; OnPropertyChanged(new PropertyChangedEventArgs("LockerID"));}
        }
        private string m_LockerName;
        public string LockerName
        {
            get{ return m_LockerName;}
            set{ m_LockerName = value; OnPropertyChanged(new PropertyChangedEventArgs("LockerName"));}
        }
        private string m_LockedTable;
        public string LockedTable
        {
            get{ return m_LockedTable;}
            set{ m_LockedTable = value; OnPropertyChanged(new PropertyChangedEventArgs("LockedTable"));}
        }
        private int m_LockedRecord;
        public int LockedRecord
        {
            get{ return m_LockedRecord;}
            set{ m_LockedRecord = value; OnPropertyChanged(new PropertyChangedEventArgs("LockedRecord"));}
        }
        private string m_LockDate;
        public string LockDate
        {
            get{ return m_LockDate;}
            set{ m_LockDate = value; OnPropertyChanged(new PropertyChangedEventArgs("LockDate"));}
        }
    }
}
