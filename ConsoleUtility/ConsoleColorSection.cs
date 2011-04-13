using System;

namespace dr.ChromePasswordRecover.ConsoleUtility
{
    /// <summary>
    /// USed to define a colored text section in a console.
    /// </summary>
    public class ConsoleColorSection : IDisposable
    {
        /// <summary>
        /// The old color.
        /// </summary>
        private readonly ConsoleColor oldColor;
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleColorSection"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public ConsoleColorSection(ConsoleColor color)
        {
            oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Console.ForegroundColor = oldColor;
        }
    }
}
