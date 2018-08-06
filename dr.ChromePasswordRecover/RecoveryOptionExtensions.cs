using System;
using dr.ChromePasswordRecover.ConsoleUtility;

namespace dr.ChromePasswordRecover
{
    public static class RecoveryOptionExtensions
    {
        public static RecoveryOptions GetRecoveryOptions(this CommandLineParser parser)
        {
            string dataFile = parser.GetSwitchValue(Switches.File);
            string outFile = parser.GetSwitchValue(Switches.Dump);
            string password = parser.GetSwitchValue(Switches.Pass);
            return new RecoveryOptions(
                dataFile, 
                outFile, 
                String.IsNullOrWhiteSpace(outFile) ? PasswordDumpFormat.Console : PasswordDumpFormat.Xml, 
                password);
        }
    }
}