using System;
using System.IO;

namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Contains the main function
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entrypoint of the program
        /// </summary>
        /// <param name="args">Commandline args</param>
        static void Main(string[] args)
        {
            string input;
            string output = "";
            switch (args.Length)
            {
                case 0:
                    //No arguments provided, prints a small usage guide
                    Console.WriteLine("Usage:");
                    Console.WriteLine("LuaAssemblyPrinter inputfile");
                    Console.WriteLine("LuaAssemblyPrinter inputfile outputfile");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                case 1:
                    //Set the input file
                    input = args[0];
                    break;
                default:
                    //Set the input and output file
                    input = args[0];
                    output = args[1];
                    break;
            }
            //Check if the input file exists
            if (!File.Exists(input))
            {
                Console.WriteLine("Input File " + input + " doesn't exist");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            try
            {
                //Create and start the printer
                AssemblyPrinter printer = new AssemblyPrinter(input);
                if (string.IsNullOrEmpty(output))
                {
                    //Print to the console of there's no output file
                    printer.Print();
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                }
                //Otherwise write everything to a file
                File.WriteAllText(output, printer.ToString());
            }
            catch (Exception e)
            {
                //Handle exceptions
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
