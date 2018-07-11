using System;
using System.Collections.Generic;
using System.Linq;

namespace dr.ChromePasswordRecover.MacOS
{
    public sealed class MacOsLoginReader : LoginReaderBase
    {
        private Memory<byte> _password = null;

        public MacOsLoginReader(string path, Memory<byte> password)
            : this(path)
        {
            _password = password;
        }

        public MacOsLoginReader(string path) : base(path)
        {
        }

        protected override IEnumerable<PlainTextLogin> DecryptPasswords(IEnumerable<Login> logins)
        {
            return logins.Select(x => x.WithPassword("password"));
        }
    }
}