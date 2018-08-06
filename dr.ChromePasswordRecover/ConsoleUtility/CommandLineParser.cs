﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace dr.ChromePasswordRecover.ConsoleUtility
{
    /// <summary>
    /// Command-line parsing helper.
    /// </summary>
    public class CommandLineParser 
    {
        /// <summary>
        /// Prefixes for switches.
        /// </summary>
        private readonly char[] switchPrefixes = {'-', '/'};
        /// <summary>
        /// The arguments as received in the constructor
        /// </summary>
        private readonly string[] args;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public CommandLineParser(string [] args)
        {
            this.args = args;
        }

        /// <summary>
        /// The arguments.
        /// </summary>
        public IEnumerable<string> Arguments
        {
            get
            {
                return from a in args
                       where !String.IsNullOrEmpty(a) && !switchPrefixes.Contains(a.First())
                       select a;
            }
        }

        /// <summary>
        /// Gets the switches.
        /// </summary>
        /// <value>The switches.</value>
        public IEnumerable<string> Switches
        {
            get
            {
                return from a in args
                       where !String.IsNullOrEmpty(a) && switchPrefixes.Contains(a.First())
                       select a.Substring(1);
            }
        }

        /// <summary>
        /// Determines whether the specified switch name has switch.
        /// </summary>
        /// <param name="switchName">Name of the switch.</param>
        /// <returns>
        /// 	<c>true</c> if the specified switch name has switch; otherwise, <c>false</c>.
        /// </returns>
        public bool HasSwitch(string switchName)
        {           
            string sw = GetNamedSwitch(switchName);
            return null != sw;
        }

        /// <summary>
        /// Gets the switch value.
        /// </summary>
        /// <param name="switchName">Name of the switch.</param>
        /// <returns></returns>
        public string GetSwitchValue(string switchName)
        {
            string sw = GetNamedSwitch(switchName);
            if (sw == null)
                return null;
            string[] parts = sw.Split(':');
            if ( parts.Length < 2)
                return null;

            return parts[1];
        }

        /// <summary>
        /// Gets the named switch.
        /// </summary>
        /// <param name="switchName">Name of the switch.</param>
        /// <returns></returns>
        private string GetNamedSwitch(string switchName)
        {
            if (switchName == null)
                throw new ArgumentNullException("switchName");

            return (from s in Switches
                   where s.StartsWith(switchName, StringComparison.OrdinalIgnoreCase)
                   select s).FirstOrDefault();
        }
    }
}
