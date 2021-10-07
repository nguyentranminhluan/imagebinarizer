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
        //private string AppLogo = "00000111000001";
        /// <summary>
        /// Main entry point for Program
        /// </summary>
        /// <param name="args">Argument of main method</param>
        static void Main(string[] args)
        {
           
            //PrintData();
            //Console.WriteLine(".------------------------------------------------------------------------------------------.");
            Console.WriteLine("\n    Welcome to Image Binarizer Application [Version 1.1.0]");
            Console.WriteLine("    Copyright <c> daenet GmbH, All rights reserved.");


            //for (int i = 1; i < 4; i++)
            //{
            //    Console.SetCursorPosition(0, i);
            //    Console.WriteLine("|");
            //    Console.SetCursorPosition(91, i);
            //    Console.WriteLine("|");
            //}

            //Console.WriteLine("\'------------------------------------------------------------------------------------------\'");          

            BinarizerConfiguration configuration;

            if (!(TryParseConfiguration(args, out configuration, out string errMsg)))
            {
                errMsg = errMsg == null ? null : "\nError: " + errMsg;
                PrintMessage(errMsg, true);
                return;
            }

            Console.WriteLine("\nImage Binarization in progress...");

            try
            {
                ImageBinarizer img = new ImageBinarizer(configuration);
                img.RunBinarizerOnLinux();
                Console.WriteLine($"{configuration.BlueThreshold}, {configuration.GreenThreshold}, {configuration.RedThreshold}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Image Binarization failed.\n");
                PrintMessage($"\nError: {e.Message}", true);
                return;
            }

            PrintMessage("\nImage Binarization completed.");
        }

        /// <summary>
        /// Print message with 
        /// </summary>
        /// <param name="errMsg"></param>
        /// <param name="isError"></param>
        private static void PrintMessage(string errMsg = null, bool isError = false)
        {
            var clr = Console.ForegroundColor;

            if (!string.IsNullOrEmpty(errMsg))
            {
                if (isError)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(errMsg + "\n");
                    Console.ForegroundColor = clr;
                    string printedHelpArgs = string.Join(", ", CommandLineParsing.HelpArguments.Select(helpArg => $"\"{helpArg}\""));
                    Console.WriteLine($"\nInsert one of these [{printedHelpArgs}] to following command for help:");
                    Console.WriteLine("\n\t\tdotnet imagebinarizer [command]\n");
                }
                else
                    Console.Write(errMsg + "\n");
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