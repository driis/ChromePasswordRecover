using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace dr.ChromePasswordRecover
{
    public abstract class LoginReaderBase : ILoginReader
    {
        /// <summary>
        /// data file location.
        /// </summary>
        private readonly string _dataFile;
        
        /// <summary>
        /// Login reader constructor
        /// </summary>
        /// <param name="path">The path.</param>
        public LoginReaderBase(string path)
        {
            _dataFile = path;
        }

        /// <summary>
        /// Connection string template.
        /// </summary>
        private const string SqLiteConnectionString = "Data Source={0}";

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
        protected abstract IEnumerable<Login> DecryptPasswords(IEnumerable<Login> logins);

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns></returns>
        private DbConnection OpenConnection()
        {
            var conn = new Microsoft.Data.Sqlite.SqliteConnection(String.Format((string) SqLiteConnectionString, (object) _dataFile));
            conn.Open();
            return conn;
        }
    }
}