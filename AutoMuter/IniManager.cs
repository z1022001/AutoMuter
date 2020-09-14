using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoMuter
{
    class IniManager
    {
        private readonly string filePath;
        private readonly int bufferSize;
        private readonly StringBuilder lpReturnedString;

        [DllImport("kernel32")] private static extern long WritePrivateProfileString(string section, string key, string lpString, string lpFileName);
        [DllImport("kernel32")] private static extern int GetPrivateProfileString(string section, string key, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        public IniManager(string iniPath)
        {
            this.filePath = iniPath;
            this.bufferSize = 1024;
            this.lpReturnedString = new StringBuilder(bufferSize);
        }

        // read ini date depend on section and key
        public string ReadIniFile(string section, string key, string defaultValue)
        {
            lpReturnedString.Clear();
            GetPrivateProfileString(section, key, defaultValue, lpReturnedString, bufferSize, filePath);
            return lpReturnedString.ToString();
        }

        // write ini data depend on section and key
        public void WriteIniFile(string section, string key, Object value)
        {
            WritePrivateProfileString(section, key, value.ToString(), filePath);
        }
    }
}
