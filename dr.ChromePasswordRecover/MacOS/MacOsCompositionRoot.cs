namespace dr.ChromePasswordRecover.MacOS
{
    class MacOsCompositionRoot : ICompositionRoot
    {
        public IChromeDataFile DataFile => new MacOsChromeDataFile();

        public ILoginReader LoginReader(string fileName, string password = null)
        {
            return new LoginReader(fileName, new MacOsCrypto(password));
        }
    }
}