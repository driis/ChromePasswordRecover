using System;
using System.IO;

namespace dr.ChromePasswordRecover.Windows
{
    class WindowsChromeDataFile : IChromeDataFile
    {
        // <summary>
        /// The relative file path for web data.
        /// </summary>
        private const string DataFileRelativePath = "Google\\Chrome\\User Data\\Default\\Login Data";

        public string LoginData
        {
            get 
            {
                string appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(appDataRoot, DataFileRelativePath);
            }
        }
    }
}