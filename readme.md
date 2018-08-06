# Google Chrome Password recovery tool

Dumps Google Chrome saved passwords for the current user.
This is a console mode program for now. Run it from the command line.


### Dual platform
This is a .NET Core application as of version 2.0 (Go back to tag "fullfx" for last full framework version)

This also means it will work on both Windows Chrome and OS X Chrome. 

For OS X it is important to note:
* Chrome cannot be running when running the app.
* The app will ask for permission to read the encryption password from the Keychain. Needless to
say, you will need to allow this for the program to work. Alternatively, you may pass the password on the commandline using the `password:` switch.

### Building
Assuming you have .NET Core 2.1 installed, you should be able to do:
```
dotnet build
```

### Usage:
        dotnet cprecover.dll <domain> <switches>

Include a domain name as the first argument if you want to get the
saved passwords for just the specified domain.

Available switches are:

  -dump:xmlfile         Dump results to the specified XML file.
  -file:filespec        Try to read passwords from the specified file.
                        If you omit this, the program will try the default
                        Chrome data directory.
  -help                 Display a help text.


### Examples: 

Print all saved logins to the console
```
dotnet cprecover.dll
``` 

Print all saved logins for www.google.com to the console:
```
dotnet cprecover.dll www.google.com
```  

Dump data to an XML file:
```
cprecover.exe -d:passwords.xml
```