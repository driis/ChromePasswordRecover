using System;
using System.Collections.Generic;
using System.Linq;

namespace dr.ChromePasswordRecover.MacOS
{
    public sealed class MacOsLoginReader : LoginReaderBase
    {
        protected override IEnumerable<PlainTextLogin> DecryptPasswords(IEnumerable<Login> logins)
        {
            return logins.Select(x => x.WithPassword("password"));
        }

        public MacOsLoginReader(string path) : base(path)
        {
        }
    }
}