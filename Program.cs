using System.Collections;
using System.Resources;
using System.Text.RegularExpressions;

namespace ResourceKeyGeneration;

partial class Program
{

    // Running from the ResourceKeyGeneration project from VS
    // setting command line parameters in VS
    // project properties / debug / open debug lunch profile
    // ..\..\..\..\HecklHelper\Resources\Resource.resx ..\..\..\..\HecklHelper\Resources\ResourceKeys.g.cs ..\..\..\..\HecklHelper\Resources\ResourcePairs.g.cs HecklHelper

    // Setting as a prebuild event without time check   
    // In the HecklHelper project (or LedgerCommander project), project properties / build / events / pre-build event
    // "..\ResourceKeyGeneration\bin\Debug\net9.0-windows\ResourceKeyGeneration.exe" Resources\Resource.resx Resources\ResourceKeys.g.cs Resources\ResourcePairs.g.cs HecklHelper

    // Setting as a prebuild event with time check
    // copy ResourceKeyGeneration.bat to the Resources folder
    // modify the parameters of the bat file
    // In the HecklHelper project (or LedgerCommander project), project properties / build / events / pre-build event
    // from HecklHelper
    //      Resources\ResourceKeyGeneration.bat ..\ResourceKeyGeneration\bin\Debug\net9.0-windows\ResourceKeyGeneration.exe Resources\Resource.resx Resources\ResourceKeys.g.cs Resources\ResourcePairs.g.cs HecklHelper
    // from LedgerCommander
    //      Resources\ResourceKeyGeneration.bat ..\..\HecklHelper\ResourceKeyGeneration\bin\Debug\net9.0-windows\ResourceKeyGeneration.exe Resources\Resource.resx Resources\ResourceKeys.g.cs Resources\ResourcePairs.g.cs LedgerCommander
    static int Main(string[] args)
    {
        var workingDirectory = Environment.CurrentDirectory;
        // Console.WriteLine($"Current Working Directory: {workingDirectory}");
        // this is the project folder

        if (args.Length != 4)
        {
            Console.WriteLine("Four parameters are needed: " 
                + "relative path of resx file, the keys output file, the pairs output file, namespace.");
            return 1; // A non-zero exit code will fail the build
        }

        // Path to your .resx file
        //var resxPath = "d:\\Heckl\\Documents\\Dropbox\\Else\\visual studio\\HecklHelper\\HecklHelper\\Resources\\Resource.resx";
        //var outputFilePath = "d:\\Heckl\\Documents\\Dropbox\\Else\\visual studio\\HecklHelper\\HecklHelper\\Resources\\ResourceKeys.g.cs";
        var resxPath = Path.GetFullPath(workingDirectory + "\\" + args[0]);
        var keysOutputFilePath = Path.GetFullPath(workingDirectory + "\\" + args[1]);
        var pairsOutputFilePath = Path.GetFullPath(workingDirectory + "\\" + args[2]);
        //Console.WriteLine($"resxPath: {resxPath}");
        //Console.WriteLine($"outputFilePath : {outputFilePath}");

        if (!File.Exists(resxPath))
        {
            Console.WriteLine($"Error: The file {resxPath} does not exist.");
            return 2;
        }

        // Needs <UseWindowsForms>true</UseWindowsForms> in the .csproj file
        // Needs <TargetFramework>net9.0-windows</TargetFramework> in the .csproj file
        // The second can be set in the project properties also
        using var resxReader = new ResXResourceReader(resxPath);
  
        using var keyWriter = new StreamWriter(keysOutputFilePath);
        using var pairWriter = new StreamWriter(pairsOutputFilePath);
        var enteredStrings = new HashSet<string>();

        keyWriter.WriteLine("// Auto-generated file for resource keys");
        keyWriter.WriteLine("namespace " + args[3] + ".Resources");
        keyWriter.WriteLine("{");
        keyWriter.WriteLine("    public static class ResourceKeys");
        keyWriter.WriteLine("    {");

        pairWriter.WriteLine("// Auto-generated file for resource pairs");
        pairWriter.WriteLine("using HecklHelper.Resources;");
        pairWriter.WriteLine("namespace " + args[3] + ".Resources");
        pairWriter.WriteLine("{");
        pairWriter.WriteLine("    public static class ResourcePairs");
        pairWriter.WriteLine("    {");

        pairWriter.WriteLine("        private static global::System.Resources.ResourceManager resourceManeger;");
        pairWriter.WriteLine("        public static global::System.Resources.ResourceManager ResourceManager");
        pairWriter.WriteLine("        {");
        pairWriter.WriteLine("            get");
        pairWriter.WriteLine("            {");
        pairWriter.WriteLine("                if (object.ReferenceEquals(resourceManeger, null))");
        pairWriter.WriteLine("                {");
        pairWriter.WriteLine("                    global::System.Resources.ResourceManager");
        pairWriter.WriteLine("                        temp = new global::System.Resources.ResourceManager(");
        pairWriter.WriteLine($"                            \"{args[3]}.Resources.Resource\", typeof(Resource).Assembly);");
        pairWriter.WriteLine("                    resourceManeger = temp;");
        pairWriter.WriteLine("                }");
        pairWriter.WriteLine("                return resourceManeger;");
        pairWriter.WriteLine("            }");
        pairWriter.WriteLine("        }");


        foreach (DictionaryEntry entry in resxReader) 
        {
            var key = entry.Key.ToString();
            var sanitizedKey = SanitizeKey(key!);
            if (!enteredStrings.Add(sanitizedKey))
            {
                Console.WriteLine($"sanitizedKey {sanitizedKey} does already exist.");
                return 3;
            }

            keyWriter.WriteLine($"        /// <summary>");
            keyWriter.WriteLine($"        /// Key for the resource '{key}'.");
            keyWriter.WriteLine($"        /// </summary>");
            keyWriter.WriteLine($"        public const string {sanitizedKey} = \"{key}\";");

            pairWriter.WriteLine($"        /// <summary>");
            pairWriter.WriteLine($"        /// Pair for the resource '{key}'.");
            pairWriter.WriteLine($"        /// </summary>");
            pairWriter.WriteLine($"        public static ResourcePair {sanitizedKey} => new(\"{key}\", ResourceManager);");
        }

        keyWriter.WriteLine("    }");
        keyWriter.WriteLine("}");

        pairWriter.WriteLine("    }");
        pairWriter.WriteLine("}");

        Console.WriteLine($"{keysOutputFilePath} and {pairsOutputFilePath} are generated.");
        return 0;
    }


    [GeneratedRegex(@"[^a-zA-Z0-9_]")]
    private static partial Regex MyRegex();

    static string SanitizeKey(string key)
    {
        // Replace invalid characters with underscores
        var sanitizedKey = MyRegex().Replace(key, "_");

        // If the key starts with a number, prepend an underscore
        if (char.IsDigit(sanitizedKey[0]))
            sanitizedKey = "_" + sanitizedKey;

        // If the key is a C# reserved keyword, append an underscore
        string[] cSharpKeywords =
        [
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private",
            "protected", "public", "readonly", "ref", "remove", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
            "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
        ];
        if (cSharpKeywords.Contains(sanitizedKey))
            sanitizedKey += "_";

        return sanitizedKey;
    }
}