using System;

namespace dr.ChromePasswordRecover.MacOS
{
    public class MacOsCrypto : ICrypto
    {
        public MacOsCrypto(string password)
        {
        }

        public string DecryptString(Memory<byte> data)
        {
            return "password";
        }
    }
}