using System.Collections.Generic;

namespace dr.ChromePasswordRecover.Windows
{
    /// <summary>
    /// Reads logins.
    /// </summary>
    public sealed class WindowsLoginReader : LoginReaderBase
    {
        /// <summary>
        /// Provides cryptographic functions.
        /// </summary>
        private static readonly CryptoApi crypto = new CryptoApi();
        
        /// <summary>
        /// Login reader constructor
        /// </summary>
        /// <param name="path">The path.</param>
        public WindowsLoginReader(string path) : base(path)
        {
        }

        /// <summary>
        /// Decrypts the passwords.
        /// </summary>
        /// <param name="logins">The logins.</param>
        /// <returns></returns>
        protected override IEnumerable<Login> DecryptPasswords(IEnumerable<Login> logins)
        {
            foreach (var login in logins)
            {
                login.Password = crypto.DecryptString(login.PasswordRaw);
                yield return login;
            }
        }
    }
}