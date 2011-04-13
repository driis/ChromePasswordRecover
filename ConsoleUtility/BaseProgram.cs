using System;

namespace dr.ChromePasswordRecover.ConsoleUtility
{
    /// <summary>
    /// Base console program class that abstracts away commonly needed functionality for console programs.
    /// </summary>
    /// <remarks>
    /// To use, inherit from BaseProgram and implement the RunProgram method.
    /// </remarks>
    public abstract class BaseProgram
    {        
        /// <summary>
        /// Command line.
        /// </summary>
        private LazyField<CommandLineParser> commandLine;        

        /// <summary>
        /// Gets the author name.
        /// </summary>
        /// <value>The author.</value>
        public virtual string Author
        {
            get
            {
                return "Dennis Riis";
            }
        }

        /// <summary>
        /// Gets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public virtual string Copyright
        {
            get
            {
                return String.Format("{0} - Copyright (c) {1}, {2}",ProductName, Author, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public virtual string ProductName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Main routine.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor">The constructor.</param>
        /// <param name="args">The args.</param>
        public static void Main<T>(Func<T> constructor, string [] args)
            where T : BaseProgram
        {
            try
            {
                if ( constructor == null )
                    throw new ArgumentNullException("constructor");
                
                var program = constructor();
                program.commandLine = LazyField.Get(() => new CommandLineParser(args));
                program.DisplayCopyright();
                program.RunProgram(args);
            }
            catch(Exception ex)
            {
                DisplayException(ex);
            }
        }

        /// <summary>
        /// Displays the copyright message.
        /// </summary>
        public virtual void DisplayCopyright()
        {
            Console.WriteLine(Copyright);
            Console.WriteLine();
        }
        /// <summary>
        /// Displays the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected static void DisplayException(Exception ex)
        {
            using(new ConsoleColorSection(ConsoleColor.Red))
            {
                Console.WriteLine("\nException occured:");
                DisplayInnerException(ex, 0);
            }
        }

        /// <summary>
        /// Displays the friendly error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected static void DisplayFriendlyError(string message)
        {
            DisplayFriendlyError(message,null);
        }

        /// <summary>
        /// Displays the friendly error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inserts">The inserts.</param>
        protected static void DisplayFriendlyError(string message, params object[] inserts)
        {
            using(new ConsoleColorSection(ConsoleColor.Red))
            {
                if ( inserts == null )
                    Console.WriteLine(message);
                else
                    Console.WriteLine(message,inserts);

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays the inner exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="count">The count.</param>
        private static void DisplayInnerException(Exception ex, int count)
        {
            if (ex == null)
                return;

            if (count > 0)
                using (new ConsoleColorSection(ConsoleColor.DarkRed))
                    Console.WriteLine("InnerException #{0}", count);
            Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
            Console.WriteLine("Stack Trace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            DisplayInnerException(ex.InnerException, count + 1);
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <param name="args">The args.</param>
        public abstract void RunProgram(string[] args);

        /// <summary>
        /// Gets the command line.
        /// </summary>
        /// <value>The command line.</value>
        public CommandLineParser CommandLine
        {
            get { return commandLine; }
        }
    }    
}
