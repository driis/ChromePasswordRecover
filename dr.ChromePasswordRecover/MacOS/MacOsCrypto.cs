using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace dr.ChromePasswordRecover.MacOS
{
    public class MacOsCrypto : ICrypto
    {
        private static readonly Regex RegPassword = new Regex("^password: \"(.*?)\"$", RegexOptions.Compiled);
        private string _password;

        public MacOsCrypto(string password)
        {
            _password = password;
        }

        public string DecryptString(Memory<byte> data)
        {
            if (_password == null)
            {
                _password = AskForPasswordFromKeyChain();
            }

            return _password;
        }

        private string AskForPasswordFromKeyChain()
        {
            var psi = new ProcessStartInfo("security", "find-generic-password -ga Chrome");
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            Process p = Process.Start(psi);
            if (p == null)
                throw new InvalidOperationException("Unable to invoke keychain to find Chrome password");
            p.WaitForExit();
            string line = null;
            while (null != (line = p.StandardError.ReadLine()))
            {
                var match = RegPassword.Match(line);
                if (match.Success)
                    return match.Groups[1].Value;
            }

            throw new InvalidOperationException("Could not find password from keychain output");
        }
    }
}