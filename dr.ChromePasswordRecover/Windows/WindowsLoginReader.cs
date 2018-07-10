using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace dr.ChromePasswordRecover.Windows
{
    /// <summary>
    /// Reads logins.
    /// </summary>
    public class WindowsLoginReader : ILoginReader
    {
        /// <summary>
        /// Connection string template.
        /// </summary>
        private const string SQLiteConnectionString = "Data Source={0}";
        /// <summary>
        /// Provides cryptographic functions.
        /// </summary>
        private static readonly CryptoApi crypto = new CryptoApi();
        /// <summary>
        /// data file location.
        /// </summary>
        private readonly string dataFile;
        /// <summary>
        /// Login reader constructor
        /// </summary>
        /// <param name="path">The path.</param>
        public WindowsLoginReader(string path)
        {
            dataFile = path;
        }

        /// <summary>
        /// Gets the logins.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public IEnumerable<Login> GetLogins(string url)
        {
            url = "%" + url + "%";
            List<Login> logins = new List<Login>();
            using(var conn = OpenConnection())
            {
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                var urlParam = command.CreateParameter();
                urlParam.DbType = DbType.String;
                urlParam.Size = url.Length;
                urlParam.ParameterName = "@url";
                urlParam.Value = url;
                command.CommandText =
                    "SELECT username_value, password_value, origin_url as Url, preferred FROM logins WHERE blacklisted_by_user = 0 AND Url LIKE @url";
                command.Parameters.Add(urlParam);
                var reader = command.ExecuteReader();
                
                while(reader.Read())
                {
                    var passwordBuffer = reader.GetValue(1) as byte[];
                    logins.Add(
                        new Login(reader.GetString(0), passwordBuffer ,reader.GetString(2), reader.GetBoolean(3))
                    );
                }
            }
            return DecryptPasswords(logins);
        }

        /// <summary>
        /// Decrypts the passwords.
        /// </summary>
        /// <param name="logins">The logins.</param>
        /// <returns></returns>
        private static IEnumerable<Login> DecryptPasswords(IEnumerable<Login> logins)
        {
            foreach (var login in logins)
            {
                login.Password = crypto.DecryptString(login.PasswordRaw);
                yield return login;
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns></returns>
        private DbConnection OpenConnection()
        {
            var conn = new Microsoft.Data.Sqlite.SqliteConnection(String.Format(SQLiteConnectionString, dataFile));
            conn.Open();
            return conn;
        }

        
    }
}