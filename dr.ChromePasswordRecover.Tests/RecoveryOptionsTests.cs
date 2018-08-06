using dr.ChromePasswordRecover.ConsoleUtility;
using Xunit;

namespace dr.ChromePasswordRecover.Tests
{
    public class RecoveryOptionsTests
    {
        [Fact]
        public void GetRecoveryOptions_NoArguments_HasDefaultValues()
        {
            CommandLineParser parser = new CommandLineParser(new string []{});

            var options = parser.GetRecoveryOptions();
            
            Assert.Equal(options.DataFile, null);
            Assert.Equal(options.OutFile, null);
            Assert.Equal(options.DecryptionPassword, null);
            Assert.Equal(options.DumpFormat, PasswordDumpFormat.Console);
        }
        
        [Theory]
        [MemberData(nameof(RecoveryParseTheories))]
        public void GetRecoveryOptions_Arguments_ParsesCorrectly(string [] args, 
            string dataFile, 
            string outFile, 
            string decryptionPassword, 
            PasswordDumpFormat dumpFormat)
        {
            CommandLineParser parser = new CommandLineParser(args);

            var options = parser.GetRecoveryOptions();
            
            Assert.Equal(options.DataFile, dataFile);
            Assert.Equal(options.OutFile, outFile);
            Assert.Equal(options.DecryptionPassword, decryptionPassword);
            Assert.Equal(options.DumpFormat, dumpFormat);
        }
        
        public static TheoryData<string[],string,string,string,PasswordDumpFormat> RecoveryParseTheories() 
        {
            var data = new TheoryData<string[],string,string,string,PasswordDumpFormat>();
            data.Add(new[]{"-f:test.data"},"test.data", null, null, PasswordDumpFormat.Console);
            data.Add(new[]{"-file:test.data"},"test.data", null, null, PasswordDumpFormat.Console);
            data.Add(new[]{"-dump:test.xml","-file:test.data"},"test.data", "test.xml", null, PasswordDumpFormat.Xml);
            data.Add(new[]{"-dump:test.xml",},null, "test.xml", null, PasswordDumpFormat.Xml);
            data.Add(new[]{"-pass:abekat",},null, null, "abekat", PasswordDumpFormat.Console);
            data.Add(new[]{"-file:test.data", "-pass:abekat",},"test.data", null, "abekat", PasswordDumpFormat.Console);
            data.Add(new[]{"-file:test.data","-d:test.xml", "-pass:abekat",},"test.data", "test.xml", "abekat", PasswordDumpFormat.Xml);
            return data;
        }
    }
}