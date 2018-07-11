using System;
using System.Security.Cryptography;
using System.Text;

namespace dr.ChromePasswordRecover.Windows
{
    /// <summary>
    /// A managed wrapper around the used Crypto API functions.
    /// </summary>
    public class DataProtectionApi : ICrypto
    {
        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public string DecryptString(Memory<byte> cipherText)
        {
            byte [] plainText = Decrypt(cipherText);
            return Encoding.UTF8.GetString(plainText);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public byte[] Decrypt(Memory<byte> cipherText)
        {
            return ProtectedData.Unprotect(cipherText.ToArray(), null, DataProtectionScope.CurrentUser);
        }      
    }
}
