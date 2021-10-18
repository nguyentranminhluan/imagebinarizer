using ImageBinarizerApp.Entities;
using ImageBinarizerLib;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageBinarizerApp
{
    /// <summary>
    /// Program Class of Console App
    /// </summary>
    class Program
    {
        /// <summary>
        /// Use ImageBinarizerLib to create binarized Logo
        /// </summary>
        private static void createBotLogo()
        {
            BinarizerConfiguration configuration = new BinarizerConfiguration();
            configuration.InputImagePath = ".\\Logo\\Logo.png";
            configuration.OutputImagePath = ".\\Logo\\Logo.txt";
            configuration.ImageWidth = 60;

            if (!(File.Exists(configuration.InputImagePath)))
            {
                return;
            }

            ImageBinarizer img = new ImageBinarizer(configuration);
            img.Run();
        }

        private static string botLogo = File.ReadAllText(".\\Logo\\dotnetbot.txt");

        /// <summary>
        /// Draw App Logo to console
        /// </summary>
        private static void PrintAppLogoAndWelcomeMessage(ConsoleColor clr, string logo)
        {
            Console.WriteLine("\nWelcome to Image Binarizer Application [Version 1.1.0]");
            Console.WriteLine("Copyright <c> daenet GmbH, All rights reserved.\n");

            var letter = (char)20;

            foreach (var c in logo)
            {
                switch (c)
                {
                    case '0':
                        Console.Write(letter);
                        break;
                    case '1':                        
                        Console.Write(' ');
                        break;
                    default:
                        Console.Write(c);
                        break;
                }
            }

            //Console.WriteLine(logo);

        }

        /// <summary>
        /// Main entry point for Program
        /// </summary>
        /// <param name="args">Argument of main method</param>
        static void Main(string[] args)
        {
            createBotLogo();

            var clr = Console.ForegroundColor;
            PrintAppLogoAndWelcomeMessage(clr, botLogo);

            if (args.Length == 0)
            {
                PrintMessage(" ", ConsoleColor.White, true);
                return;
            }

            BinarizerConfiguration configuration;

            if (!(TryParseConfiguration(args, out configuration, out string errMsg)))
            {
                errMsg = errMsg == null ? null : "\nError: " + errMsg;
                PrintMessage(errMsg, ConsoleColor.Red, true);
                return;
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nImage Binarization in progress...");
            Console.ForegroundColor = clr;

            try
            {
                ImageBinarizer img = new ImageBinarizer(configuration);
                img.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("Image Binarization failed.\n");
                PrintMessage($"\nError: {e.Message}", ConsoleColor.Red, true);
                return;
            }

            PrintMessage($"\nImage Binarization completed. Your Binarized Image is saved at:\n\t{configuration.OutputImagePath}", ConsoleColor.Green);

        }

        /// <summary>
        /// Print message 
        /// </summary>
        /// <param name="msg">string of message</param>
        /// <param name="isError">to check if this is an error message</param>
        private static void PrintMessage(string msg = null, ConsoleColor clr = ConsoleColor.White, bool isError = false)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (isError)
                {
                    Console.ForegroundColor = clr;
                    Console.Write(msg + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    string printedHelpArgs = string.Join(", ", CommandLineParsing.HelpArguments.Select(helpArg => $"\"{helpArg}\""));
                    Console.WriteLine($"\nInsert one of these [{printedHelpArgs}] to following command for help:");
                    Console.WriteLine("\n\t\timgbin [command]\n");
                }
                else
                {
                    Console.ForegroundColor = clr;
                    Console.Write(msg + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary>
        /// Check validation of arguments
        /// </summary>
        /// <param name="args">input arguments</param>
        /// <param name="configurationData">Configurations provided</param>
        /// <returns></returns>
        private static bool TryParseConfiguration(string[] args, out BinarizerConfiguration configurationData, out string errMsg)
        {
            var parsingObject = new CommandLineParsing(args);

            //
            // Check if Parsed data is correct
            return parsingObject.Parse(out configurationData, out errMsg);
        }

    }
}

// --input-image D:\DAENET\image\daenet.png --output-image D:\DAENET\image\out.txt -iw 800 -ih 225 -rt 100 -gt 100 -bt 100