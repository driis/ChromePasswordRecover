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
        /// <param name="passwordRaw">The password raw.</param>
        /// <param name="url">The URL.</param>
        public Login(string userName, byte[] passwordRaw, string url, bool preferred)
        {
            UserName = userName;
            PasswordRaw = passwordRaw;
            Url = url;
            Preferred = preferred;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; internal set; }

        /// <summary>
        /// Gets or sets the password raw.
        /// </summary>
        /// <value>The password raw.</value>
        public byte[] PasswordRaw { get; private set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Login"/> is preferred.
        /// </summary>
        /// <value><c>true</c> if preferred; otherwise, <c>false</c>.</value>
        public bool Preferred { get; private set; }
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
}
