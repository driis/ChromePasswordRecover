using System;

namespace dr.ChromePasswordRecover
{
    public interface ICrypto
    {
        string DecryptString(Memory<byte> data);
    }
}