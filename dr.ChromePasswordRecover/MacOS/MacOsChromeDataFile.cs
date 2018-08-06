using System;
using System.IO;

namespace dr.ChromePasswordRecover.MacOS
{
    internal class MacOsChromeDataFile : IChromeDataFile
    {
        public string LoginData 
        {
            get
            {
                string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                return Path.Combine(home, "Library/Application Support/Google/Chrome/Default/Login Data");
            }
        }
    }
}