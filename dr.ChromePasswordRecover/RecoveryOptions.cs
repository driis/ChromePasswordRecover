namespace dr.ChromePasswordRecover
{
    public class RecoveryOptions
    {
        public RecoveryOptions(string dataFile,
            string outFile = null,
            PasswordDumpFormat dumpFormat = PasswordDumpFormat.Console,
            string decryptionPassword = null)
        {
            DataFile = dataFile;
            DecryptionPassword = decryptionPassword;
            OutFile = outFile;
            DumpFormat = dumpFormat;
        }

        public string DataFile { get; }
        public string DecryptionPassword { get; }
        public string OutFile { get; }
        public PasswordDumpFormat DumpFormat {get;}
    }

    public enum PasswordDumpFormat
    {
        Console, 
        Xml,
        Json
    }
}