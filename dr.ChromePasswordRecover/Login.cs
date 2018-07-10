using System;

namespace dr.ChromePasswordRecover
{
    /// <summary>
    /// Defines a login.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="encryptedPassword">The password raw.</param>
        /// <param name="url">The URL.</param>
        /// <param name="preferred">Preferred lign?</param>
        public Login(string userName, byte[] encryptedPassword, string url, bool preferred)
        {
            UserName = userName;
            EncryptedPassword = encryptedPassword;
            Url = url;
            Preferred = preferred;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; }

        /// <summary>
        /// Gets or sets the password raw.
        /// </summary>
        /// <value>The password raw.</value>
        public byte[] EncryptedPassword { get; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Login"/> is preferred.
        /// </summary>
        /// <value><c>true</c> if preferred; otherwise, <c>false</c>.</value>
        public bool Preferred { get; }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} @ {1}", UserName, Url);
        }
    }

    public class PlainTextLogin : Login
    {
        public PlainTextLogin(string userName, byte[] encryptedPassword, string url, bool preferred, string password) : base(userName, encryptedPassword, url, preferred)
        {
            Password = password;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; }
    }

    public static class PlainTextLoginExtension
    {
        public static PlainTextLogin WithPassword(this Login login, string plainTextPassword)
        {
            return new PlainTextLogin(login.UserName, login.EncryptedPassword, login.Url, login.Preferred, plainTextPassword);
        }
    }
}
