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
        //public static void PrintData()
        //{
        //    var clr = Console.ForegroundColor;
        //    using (StreamReader readtext = new StreamReader("out1.txt"))
        //    {
        //        string readMeText = readtext.ReadLine();
        //        while (readMeText != null)
        //        {
        //            for (int i = 0; i < readMeText.Length; i++)
        //            {
        //                if (readMeText[i] == '0')
        //                {
        //                    Console.ForegroundColor = ConsoleColor.Blue;
        //                }

        //                Console.Write(readMeText[i]);
        //                Console.ForegroundColor = clr;
        //            }

        //            Console.WriteLine();
        //            readMeText = readtext.ReadLine();
        //        }

        //    }
        //}
        //
        private static string appLogo = "11111100033330\n" +
                                    "00110000033033\n" +
                                    "00110000033033\n" +                                    
                                    "00110222223330\n" +
                                    "00110202023033\n" +
                                    "00110202023033\n" +                                    
                                    "11111102023330\n";

        /// <summary>
        /// Draw App Logo to console
        /// </summary>
        private static void PrintAppLogo(ConsoleColor clr) {            
            var letter = (char)20;
            foreach (var c in appLogo)
            {
                switch (c)
                {
                    case '0':
                        Console.ForegroundColor = clr;
                        Console.Write(' ');
                        break;
                    case '1':
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(letter);
                        break;
                    case '2':
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(letter);
                        break;
                    case '3':
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(letter);
                        break;
                    default:
                        Console.ForegroundColor = clr;
                        Console.Write(c);
                        break;
                }                    
            }
        }

        /// <summary>
        /// Main entry point for Program
        /// </summary>
        /// <param name="args">Argument of main method</param>
        static void Main(string[] args)
        {
            var clr = Console.ForegroundColor;

            Console.WriteLine("\nWelcome to Image Binarizer Application [Version 1.1.0]");
            Console.WriteLine("Copyright <c> daenet GmbH, All rights reserved.\n");
            PrintAppLogo(clr);     

            BinarizerConfiguration configuration;

            if (!(TryParseConfiguration(args, out configuration, out string errMsg)))
            {
                errMsg = errMsg == null ? null : "\nError: " + errMsg;
                PrintMessage(errMsg, true, ConsoleColor.Red);
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
                PrintMessage($"\nError: {e.Message}", true, ConsoleColor.Red);
                return;
            }

            PrintMessage($"\nImage Binarization completed. Your Binarized Image is saved at:\n\t{configuration.OutputImagePath}",false, ConsoleColor.Green);
            
        }

        /// <summary>
        /// Print message with 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isError"></param>
        private static void PrintMessage(string msg = null, bool isError = false, ConsoleColor clr = ConsoleColor.White)
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
                    Console.WriteLine("\n\t\tdotnet imagebinarizer [command]\n");
                }
                else
                {
                    Console.ForegroundColor = clr;
                    Console.Write(msg + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.WriteLine("\nPress any key to exit the application.");
            Console.ReadLine();
        }

        /// <summary>
        /// Check validation of arguments
        /// </summary>
        /// <param name="args"></param>
        /// <param name="configurationDatas"></param>
        /// <returns></returns>
        private static bool TryParseConfiguration(string[] args, out BinarizerConfiguration configurationDatas, out string errMsg)
        {
            var parsingObject = new CommandLineParsing(args);

            //
            // Check if datas Parsed is correct
            return parsingObject.Parse(out configurationDatas, out errMsg);
        }

    }
}

// --input-image D:\DAENET\image\daenet.png --output-image D:\DAENET\image\out.txt -width 800 -height 225 -red 100 -green 100 -blue 100