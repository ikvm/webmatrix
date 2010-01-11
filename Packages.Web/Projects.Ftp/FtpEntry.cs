namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using System;
    using System.Globalization;

    internal sealed class FtpEntry
    {
        private FtpEntryType _entryType;
        private string _fileName;
        private long _fileSize;
        private DateTime _modifiedDate;
        private static readonly char[] Whitespace = new char[] { ' ', '\t' };

        private FtpEntry()
        {
        }

        public static FtpEntry CreateEntry(string entryText)
        {
            entryText = entryText.Trim();
            char c = entryText[0];
            if (char.IsDigit(c))
            {
                return ParseDosFormat(entryText);
            }
            if ((c != 'd') && (c != '-'))
            {
                return null;
            }
            return ParseUnixFormat(entryText);
        }

        private static bool IsMonth(string s)
        {
            if (s.Length != 3)
            {
                return false;
            }
            s = s.ToLower();
            if ((((!s.Equals("jan") && !s.Equals("feb")) && (!s.Equals("mar") && !s.Equals("apr"))) && ((!s.Equals("may") && !s.Equals("jun")) && (!s.Equals("jul") && !s.Equals("aug")))) && ((!s.Equals("sep") && !s.Equals("oct")) && (!s.Equals("nov") && !s.Equals("dec"))))
            {
                return false;
            }
            return true;
        }

        private static FtpEntry ParseDosFormat(string entryText)
        {
            FtpEntry entry = new FtpEntry();
            string[] strArray = new string[3];
            int index = 0;
            int startIndex = 0;
            for (int i = entryText.IndexOfAny(Whitespace, startIndex); index < 3; i = entryText.IndexOfAny(Whitespace, startIndex))
            {
                strArray[index] = entryText.Substring(startIndex, i - startIndex);
                index++;
                startIndex = i;
                while (char.IsWhiteSpace(entryText[startIndex]))
                {
                    startIndex++;
                }
            }
            string str = strArray[0] + " " + strArray[1];
            if (char.ToLower(str[str.Length - 1], CultureInfo.InvariantCulture) != 'm')
            {
                str = str + "m";
            }
            try
            {
                entry._modifiedDate = Convert.ToDateTime(str);
            }
            catch (Exception)
            {
            }
            if (string.Compare(strArray[2], "<DIR>", false, CultureInfo.InvariantCulture) == 0)
            {
                entry._entryType = FtpEntryType.Directory;
            }
            else
            {
                entry._entryType = FtpEntryType.File;
                try
                {
                    entry._fileSize = long.Parse(strArray[2]);
                }
                catch (Exception)
                {
                }
            }
            entry._fileName = entryText.Substring(startIndex);
            return entry;
        }

        private static FtpEntry ParseUnixFormat(string entryText)
        {
            string str2;
            string str3;
            FtpEntry entry = new FtpEntry();
            string[] strArray = new string[8];
            int index = 0;
            int startIndex = 0;
            for (int i = entryText.IndexOfAny(Whitespace, startIndex); (index < 8) && (i > 0); i = entryText.IndexOfAny(Whitespace, startIndex))
            {
                strArray[index] = entryText.Substring(startIndex, i - startIndex);
                index++;
                startIndex = i;
                while (char.IsWhiteSpace(entryText[startIndex]))
                {
                    startIndex++;
                }
            }
            bool flag = false;
            if (IsMonth(strArray[5]))
            {
                flag = true;
            }
            else if (!IsMonth(strArray[4]))
            {
                return null;
            }
            if (entryText[0] == 'd')
            {
                entry._entryType = FtpEntryType.Directory;
            }
            else
            {
                entry._entryType = FtpEntryType.File;
            }
            string s = flag ? strArray[4] : strArray[3];
            try
            {
                entry._fileSize = long.Parse(s);
            }
            catch (Exception)
            {
            }
            if (flag)
            {
                str2 = strArray[6] + " " + strArray[5];
                str3 = strArray[7];
            }
            else
            {
                str2 = strArray[5] + " " + strArray[4];
                str3 = strArray[6];
            }
            if (str3.IndexOf(':') < 0)
            {
                str2 = str2 + " " + str3;
            }
            else
            {
                str2 = string.Concat(new object[] { str2, " ", DateTime.Today.Year, " ", str3 });
            }
            try
            {
                entry._modifiedDate = Convert.ToDateTime(str2);
            }
            catch (Exception)
            {
            }
            entry._fileName = entryText.Substring(startIndex);
            return entry;
        }

        public FtpEntryType EntryType
        {
            get
            {
                return this._entryType;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
        }

        public long FileSize
        {
            get
            {
                if (this._entryType == FtpEntryType.File)
                {
                    return this._fileSize;
                }
                return 0L;
            }
        }

        public DateTime Modified
        {
            get
            {
                return this._modifiedDate;
            }
        }
    }
}

