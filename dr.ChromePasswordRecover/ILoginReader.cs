using System.Collections.Generic;
using System.IO;

namespace dr.ChromePasswordRecover
{
    public interface ILoginReader
    {
        IEnumerable<PlainTextLogin> GetLogins(string url);       
    }
}
