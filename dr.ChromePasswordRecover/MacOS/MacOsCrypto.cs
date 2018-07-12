using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace dr.ChromePasswordRecover.MacOS
{
    public class MacOsCrypto : ICrypto
    {
        private const byte PaddingChar = 8;
        private static readonly Regex RegPassword = new Regex("^password: \"(.*?)\"$", RegexOptions.Compiled);
        private string _password = null;
        private byte[] _encryptionKey = null;
        private static readonly Aes Aes = Aes.Create();
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("saltysalt");
        private static readonly byte[] Iv = {0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20};
        private static int Iterations = 1003;
        private static readonly Memory<byte> Version = Encoding.ASCII.GetBytes("v10"); 
        public MacOsCrypto(string password)
        {
            _password = password;
            Aes.BlockSize = 128;
            Aes.Mode = CipherMode.CBC;
            Aes.Padding = PaddingMode.None;
        }

        public string DecryptString(Memory<byte> secret)
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

            var inputBuffer = secret.Span;
            if (!inputBuffer.StartsWith(Version.Span))
                return null;    // Not something we understand
            inputBuffer = inputBuffer.Slice(Version.Length);
            var outputBuffer = new Span<byte>(new byte[inputBuffer.Length]);
            
            using (var decryptor = Aes.CreateDecryptor(_encryptionKey, Iv))
            using(var input = new MemoryStream(inputBuffer.ToArray()))
            using(var cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.Read(outputBuffer);
                int unpaddedLength = outputBuffer.IndexOf(PaddingChar);
                if (unpaddedLength > 0)     // -1 if original password not padded, e.g. fit a block exactly
                    outputBuffer = outputBuffer.Slice(0, unpaddedLength);
                return Encoding.UTF8.GetString(outputBuffer);
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