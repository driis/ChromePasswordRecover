using System.Reflection.Metadata.Ecma335;

namespace dr.ChromePasswordRecover.Windows
{
    class WindowsCompositionRoot : ICompositionRoot
    {
        public IChromeDataFile DataFile => new WindowsChromeDataFile();
        public ILoginReader LoginReader(string fileName, string password = null)
        {
            return new LoginReader(fileName, new DataProtectionApi());
        }
    }
}