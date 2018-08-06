using System;
using System.Runtime.InteropServices;
using dr.ChromePasswordRecover.MacOS;
using dr.ChromePasswordRecover.Windows;

namespace dr.ChromePasswordRecover
{
    public interface ICompositionRoot
    {
        IChromeDataFile DataFile { get; }
        ILoginReader LoginReader(string fileName, string password = null);
    }

    public static class CompositionRoot
    {
        private static readonly Lazy<ICompositionRoot> _current = new Lazy<ICompositionRoot>(ChooseComposer);
        public static ICompositionRoot Current => _current.Value;
        
        private static ICompositionRoot ChooseComposer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return new MacOsCompositionRoot();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new WindowsCompositionRoot();
            throw new InvalidOperationException($"Current OS not supported: {RuntimeInformation.OSDescription}");
        }
    }
}