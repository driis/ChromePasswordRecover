using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace dr.ChromePasswordRecover.MacOS
{
    public class MacOsCrypto : ICrypto
    {
        private static readonly Regex RegPassword = new Regex("^password: \"(.*?)\"$", RegexOptions.Compiled);
        private string _password = null;
        private byte[] _encryptionKey = null;
        private static readonly Aes Aes = Aes.Create();
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("saltysalt");
        private static readonly byte[] Iv = new byte[16] {20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20};
        private static int Iterations = 1003;
        
        public MacOsCrypto(string password)
        {
            _password = password;
            Aes.Padding = PaddingMode.None;
        }

        public string DecryptString(Memory<byte> data)
        {
            if (_password == null)
            {
                _password = AskForPasswordFromKeyChain();
            }

            if (_encryptionKey == null)
            {
                _encryptionKey = KeyDerivation.Pbkdf2(_password, Salt,
                    KeyDerivationPrf.HMACSHA1, Iterations, 16);
            }

            using (var decryptor = Aes.CreateDecryptor(_encryptionKey, Iv))
            {
                int blocks = data.Length / decryptor.InputBlockSize;                           
                blocks = blocks + (data.Length % decryptor.InputBlockSize == 0 ? 0 : 1);
                byte[] buffer = new byte[blocks * decryptor.InputBlockSize];
                data.CopyTo(buffer);
                using(var input = new MemoryStream(buffer))
                using(var cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                using(var reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }

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