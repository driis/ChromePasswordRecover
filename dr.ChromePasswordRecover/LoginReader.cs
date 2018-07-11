using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace dr.ChromePasswordRecover
{
    public class LoginReader : ILoginReader
    {
        /// <summary>
        /// data file location.
        /// </summary>
        private readonly string _dataFile;

        private readonly ICrypto _crypto;

        /// <summary>
        /// Login reader constructor
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="crypto">Cryptographic implementation</param>
        public LoginReader(string path,  ICrypto crypto)
        {
            _dataFile = path;
            _crypto = crypto;
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
        public IEnumerable<PlainTextLogin> GetLogins(string url)
        {
            url = "%" + url + "%";
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

                return LoginsFromReader(reader).ToArray();
            }
        }

        private IEnumerable<PlainTextLogin> LoginsFromReader(DbDataReader reader)
        {
            while (reader.Read())
            {
                var passwordBuffer = reader.GetValue(1) as byte[];
                string password = _crypto.DecryptString(passwordBuffer);
                yield return new PlainTextLogin(
                    reader.GetString(0), 
                    passwordBuffer, 
                    reader.GetString(2),
                    reader.GetBoolean(3), 
                    password);

            }
        }

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