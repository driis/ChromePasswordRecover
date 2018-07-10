namespace dr.ChromePasswordRecover.MacOS
{
    class MacOsCompositionRoot : ICompositionRoot
    {
        public IChromeDataFile DataFile => new MacOsChromeDataFile();

        public ILoginReader LoginReader(string fileName)
        {
            return new MacOsLoginReader(fileName);
        }
    }
}