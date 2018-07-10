using System;
using System.Collections.Generic;

namespace dr.ChromePasswordRecover.MacOS
{
    public sealed class MacOsLoginReader : LoginReaderBase
    {
        protected override IEnumerable<Login> DecryptPasswords(IEnumerable<Login> logins)
        {
            return logins;
        }

        public MacOsLoginReader(string path) : base(path)
        {
        }
    }
}