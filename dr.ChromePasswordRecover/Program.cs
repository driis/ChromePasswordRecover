using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using dr.ChromePasswordRecover.ConsoleUtility;

namespace dr.ChromePasswordRecover
{
    /// <summary>
    /// Main program class.
    /// </summary>
    class Program : BaseProgram
    {
        private const string format = "{0,-30} {1,-18} {2}";

        /// <summary>
        /// Main entry point for the program dr.ChromePasswordRecover
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static void Main(string[] args)
        {
            Main(() => new Program(), args);
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void RunProgram(string[] args)
        {           
            CommandLineParser parser = new CommandLineParser(args);            
            if ( parser.HasSwitch(Switches.Help))
            {
                DisplayUsage();
                return;
            }

            string dataFile = parser.GetSwitchValue(Switches.File);
            if (dataFile == null)
                dataFile = LoginReader.GetDefaultChromePasswordFile();

            if (!File.Exists(dataFile))
            {
                DisplayFriendlyError("File does not exist: {0}", dataFile);
                return;
            }

            // Copy the file to the temp dir. In most cases, this will let us run the LoginReader even if Chrome is running.
            string filename = Path.GetTempFileName();
            File.Copy(dataFile, filename, true);
            try
            {
                LoginReader reader = new LoginReader(filename);
                var logins = reader.GetLogins(parser.Arguments.FirstOrDefault())
                    .Where(l => !String.IsNullOrEmpty(l.UserName));
                var dumpFile = parser.GetSwitchValue(Switches.Dump);
                if (!String.IsNullOrEmpty(dumpFile))
                    WriteAsXml(dumpFile, logins);
                else
                    WriteToConsole(logins);

                Console.WriteLine();
            }
            catch 
            {
                // Delete the temp file to be a good citizen :-)
                try
                {
                    File.Delete(filename);
                }
                catch (Exception) {}
                throw;
            }
        }

        /// <summary>
        /// Writes as XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="logins">The logins.</param>
        private static void WriteAsXml(string fileName, IEnumerable<Login> logins)
        {
            Console.WriteLine("Dumping passwords to file: {0}", fileName);
            var data = logins.GroupBy(login => login.Url, StringComparer.OrdinalIgnoreCase);
            XDocument xdoc = new XDocument(
                new XElement("chrome-credentials-dump",
                             from url in data
                             orderby url.Key
                             select new XElement("url", new XAttribute("href", url.Key), 
                                                 from credential in url
                                                 orderby credential.Preferred descending
                                                 select new XElement("credential",
                                                                     new XAttribute("preferred", credential.Preferred),
                                                                     new XElement("userName",
                                                                                  new XCData(credential.UserName)),
                                                                     new XElement("password",
                                                                                  new XCData(credential.Password))))));
            try
            {
                xdoc.Save(fileName);
            }
            catch
            {
                DisplayFriendlyError("Error while saving to file: {0}", fileName);               
            }
            Console.WriteLine("Done.");
        }

        /// <summary>
        /// Writes to console.
        /// </summary>
        /// <param name="logins">The logins.</param>
        private static void WriteToConsole(IEnumerable<Login> logins)
        {
            Console.WriteLine(format, "URL", "User name", "Password");
            Console.WriteLine(format, new String('-',30), new String('-',18), new String('-',29));
                                                           
            foreach(var url in logins.GroupBy(login => login.Url, StringComparer.OrdinalIgnoreCase).OrderBy(u => u.Key))
            {
                Console.WriteLine(format, url.Key.Left(30,"..."), url.First().UserName.Left(18, "..."),url.First().Password);
                foreach(var cred in url.Skip(1))
                    Console.WriteLine(format,null, cred.UserName.Left(18,"..."),cred.Password);
            }
        }


        /// <summary>
        /// Displays the usage info message.
        /// </summary>
        private static void DisplayUsage()
        {
            Console.WriteLine("Dumps Google Chrome saved passwords for the current user.\n");
            Console.WriteLine("Usage:\n\tcprecover.exe <domain> <switches>\n");
            Console.WriteLine("Include a domain name as the first argument if you want to get the\nsaved passwords for just the specified domain.\n ");
            Console.WriteLine("Available switches are:\n");
            Console.WriteLine("  -dump:xmlfile\t\tDump results to the specified XML file.");
            Console.WriteLine("  -file:filespec\tTry to read passwords from the specified file. \n\t\t\tIf you omit this, the program will try the default\n\t\t\tChrome data directory.");
            Console.WriteLine("  -help\t\t\tDisplay this help text.");
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public override string ProductName
        {
            get { return "Google Chrome Password recovery tool"; }
        }

        /// <summary>
        /// Defines available switches.
        /// </summary>
        private class Switches
        {
            public const string Help = "h";
            public const string Dump = "d";
            public const string File = "f";
        }        
    }
}