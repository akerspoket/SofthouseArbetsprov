using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse
{
    // Our exception class
    [System.Serializable]
    public class InvalidArguments : Exception
    {
        public InvalidArguments() { }
        public InvalidArguments(string message) : base(message) { }
        public InvalidArguments(string message, Exception inner) : base(message, inner) { }
        protected InvalidArguments(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // See if we are building a stand alone exe
#if STANDALONE
            bool shouldContinue = true;
            M_WriteHelp();
            while (shouldContinue)
            {
                // Get the input from console read
                Console.Write("Input command >");
                string command = Console.ReadLine();
                args = command.Split(' ');
                shouldContinue = !Array.Exists(args, x => x.ToLower() == "-q");
                M_RunCommands(args);
            }
#else
            M_RunCommands(args);
#endif
        }

        private static void M_RunCommands(string[] args)
        {
            try
            {
                // See which commands are given
                
                // See if it is help
                bool showHelp = Array.Exists(args, x => x.ToLower() == "-h");
                if (showHelp)
                {
                    M_WriteHelp();
                }

                // See if it is an entire folder that needs converting
                int index = Array.FindIndex(args, x => x.ToLower() == "-f");
                if (index != -1)
                {
                    // It's an entire folder, make sure the user have input a folder location
                    if (args.Length <= index + 1)
                    {
                        throw new InvalidArguments("No folder followed -f command. Write -h for help");
                    }
                    // See if the user is providing a file ending
                    int endingIndex = Array.FindIndex(args, x => x.ToLower() == "-e");
                    if (endingIndex != -1)
                    {
                        // Make sure -e is followed by something
                        if (args.Length <= endingIndex + 1)
                        {
                            throw new InvalidArguments("No ending followed -e command. Write -h for help");
                        }
                        else
                        {
                            M_ParseFolder(args[index + 1], args[endingIndex + 1]);
                        }
                    }
                    else
                    {
                        M_ParseFolder(args[index + 1]);
                    }
                }
                else
                {
                    // Find the input file
                    index = Array.FindIndex(args, x => x.ToLower() == "-i");
                    if (index == -1)
                    {
                        throw new InvalidArguments("No input file. Write -h for help");
                    }
                    // Make sure the -i is followed by something
                    if (args.Length <= index + 1)
                    {
                        throw new InvalidArguments("No file followed -i command. Write -h for help");
                    }
                    string inputFile = args[index + 1];

                    // Find the output file
                    index = Array.FindIndex(args, x => x.ToLower() == "-o");
                    if (index == -1)
                    {
                        throw new InvalidArguments("No output file. Write -h for help");
                    }
                    // Make sure the -o is followed by something
                    if (args.Length <= index + 1)
                    {
                        throw new InvalidArguments("No file followed -o command. Write -h for help");
                    }
                    string outputFile = args[index + 1];

                    // When we are sure we have a input and output file, we parse
                    M_ParseFile(inputFile, outputFile);
                }

            }
            catch (InvalidArguments e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.FileName + " not found");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }

        private static void M_WriteHelp()
        {
            Console.WriteLine("Use the application in the following way");
            Console.WriteLine("-f <relative folder location>                            To parse the entire content of a folder");
            Console.WriteLine("-i <relative input file> -o <relative output file>       To parse a single file");
            Console.WriteLine("-e <file ending>                                         With -f to only parse files with specific ending");
            Console.WriteLine("-h                                                       To show this message");
            Console.WriteLine("-q                                                       To quit the application");

        }

        private static void M_ParseFolder(string folder, string ending = "")
        {
            // Get all files in the given directory
            string[] files = Directory.GetFiles(folder);
            foreach (var item in files)
            {
                // We always add parsed to the end of a parsed file, so make sure the file have not already been parsed
                // If an ending is provided, this will make sure the ending is in the file name 
                if (item.EndsWith("parsed") || !item.EndsWith(ending))
                {
                    continue;
                }
                // Parse the file
                // Should we catch errors here and continue with the next file 
                M_ParseFile(item, item + "parsed");
            }
        }

        private static void M_ParseFile(string inputFile, string outputFile)
        {
            // Open the file and read it into the string array containing the unparsed text
            string[] input = File.ReadAllLines(inputFile);

            // Create the parser class
            Parser parser = new Parser();

            // Parse lines
            parser.M_ParseLines(input);

            // Save to file
            parser.M_SaveParsedLines(outputFile);
        }
    }
}
