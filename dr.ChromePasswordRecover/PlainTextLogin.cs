using System;

namespace dr.ChromePasswordRecover
{
    public static class PlainTextLoginExtension
    {
        public static PlainTextLogin WithPassword(this Login login, string plainTextPassword)
        {
            return new PlainTextLogin(login.UserName, login.EncryptedPassword, login.Url, login.Preferred, plainTextPassword);
        }
    }

    public class PlainTextLogin : Login
    {
        public PlainTextLogin(string userName, Memory<byte> encryptedPassword, string url, bool preferred, string password) : base(userName, encryptedPassword, url, preferred)
        {
            Password = password;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; }
    }
}