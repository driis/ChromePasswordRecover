Google Chrome Password recovery tool - Copyright (c) Dennis Riis, 2012

Dumps Google Chrome saved passwords for the current user.
This is a console mode program for now. Run it from the command line.


Usage:
        cprecover.exe <domain> <switches>

Include a domain name as the first argument if you want to get the
saved passwords for just the specified domain.

Available switches are:

  -dump:xmlfile         Dump results to the specified XML file.
  -file:filespec        Try to read passwords from the specified file.
                        If you omit this, the program will try the default
                        Chrome data directory.
  -help                 Display this help text.


Examples: 

  ## Print all saved logins to the console
  cprecover.exe

  ## Print all saved logins for www.google.com to the console:
  cprecover.exe www.google.com

  ## Dump data to an XML file:
  cprecover.exe -d:passwords.xml