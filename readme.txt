In C# Resource.AccountNameInHistory returns the locale string. We generate the ResourcePairs file where ResourcePairs.AccountNameInHistory returns a class which contains both the locale and the English string. 

For me it is useful because I have a localized program but the log file must contain local and English messages as well.

USAGE
Add the ResourcePair.cs and ResourceKeyGeneration.bat to your project into the Resources folder
In the pairWriter.WriteLine("using HecklHelper.Resources;"); row change HecklHelper
Compile ResourceKeyGeneration.cs
add a prebuild event to your project
e.g.: Resources\ResourceKeyGeneration.bat ..\ResourceKeyGeneration\bin\Debug\net9.0-windows\ResourceKeyGeneration.exe Resources\Resource.resx Resources\ResourceKeys.g.cs Resources\ResourcePairs.g.cs HecklHelper
When you compile your program ResourcePairs.g.cs and ResourceKeys.g.cs should appear in the Resource folder