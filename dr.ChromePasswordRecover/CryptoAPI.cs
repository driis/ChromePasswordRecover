using System;
using System.Security.Cryptography;
using System.Text;

namespace dr.ChromePasswordRecover
{
    /// <summary>
    /// A managed wrapper around the used Crypto API functions.
    /// </summary>
    public class CryptoAPI
    {
        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public string DecryptString(byte [] cipherText)
        {
            byte [] plainText = Decrypt(cipherText);
            return Encoding.UTF8.GetString(plainText);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte [] cipherText)
        {
            if (cipherText == null)
                throw new ArgumentNullException("cipherText");

            return ProtectedData.Unprotect(cipherText, null, DataProtectionScope.CurrentUser);
        }      
    }
}
