using ImageBinarizerApp.Entities;
using System;
using System.IO;
using System.Linq;

namespace ImageBinarizerApp
{
    /// <summary>
    /// Program Class of Console App
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point for Program
        /// </summary>
        /// <param name="args">Argument of main method</param>
        static void Main(string[] args)
        {
            string printedHelpArgs = string.Join(", ", CommandLineParsing.HelpArguments.Select(helpArg => $"\"{helpArg}\""));
            Console.WriteLine(".------------------------------------------------------------------------------------------.");
            Console.WriteLine("\n    Welcome to Image Binarizer Application [Version 1.0.2]|");
            Console.WriteLine("    Copyright <c> 2019 daenet GmbH, Damir Dobric. All rights reserved.");
            Console.WriteLine($"\n    Insert one of these [{printedHelpArgs}] to following command for help:");
            Console.WriteLine("\n\t\tdotnet ImageBinarizerApp [command]\n|");
            for (int i = 1; i < 9; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("|");
                Console.SetCursorPosition(91, i);
                Console.WriteLine("|");
            }
            Console.WriteLine("\'------------------------------------------------------------------------------------------\'");

            #region Old code
            //Test if necessary input arguments were supplied.
            //if (args.length < 8)
            //{
            //    if (args.length == 1 && args[0].equals("-help"))
            //    {
            //        console.writeline("\nhelp:");
            //        console.writeline("\npass the arguments as following:");
            //        console.writeline("\nexample with automatic rgb:\ndotnet imagebinarizerapp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
            //        console.writeline("\nexample with explicit rgb:\ndotnet imagebinarizerapp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 -red 100 -green 100 -blue 100");
            //    }
            //    else
            //    {
            //        console.writeline("\nerror: all necessary arguments are not passed. please pass the arguments first.");
            //    }
            //    console.writeline("\npress any key to exit the application.");
            //    console.readline();
            //    return;
            //}

            //String inputImagePath = "";
            //String outputImagePath = "";
            //int imageWidth = 0;
            //int imageHeight = 0;
            //int redThreshold = -1;
            //int greenThreshold = -1;
            //int blueThreshold = -1;

            //if (!(args[0].Equals("--input-image") && File.Exists(args[1])))
            //{
            //    Console.WriteLine("\nError: Input file doesn't exist.");
            //    Console.WriteLine("\nPress any key to exit the application.");
            //    Console.ReadLine();
            //    return;
            //}


            //inputImagePath = args[1];


            //int separatorIndex = args[3].LastIndexOf(Path.DirectorySeparatorChar);
            //if (!(args[2].Equals("--output-image") && separatorIndex >= 0 && Directory.Exists(args[3].Substring(0, separatorIndex))))
            //{
            //    Console.WriteLine("\nError: Output Directory doesn't exist.");
            //    Console.WriteLine("\nPress any key to exit the application.");
            //    Console.ReadLine();
            //    return;
            //}

            //outputImagePath = args[3];


            //if (!args[4].Equals("-width") || !int.TryParse(args[5], out imageWidth))
            //{
            //    Console.WriteLine("\nError: Image Width should be integer.");
            //    Console.WriteLine("\nPress any key to exit the application.");
            //    Console.ReadLine();
            //    return;
            //}

            //if (!args[6].Equals("-height") || !int.TryParse(args[7], out imageHeight))
            //{
            //    Console.WriteLine("\nError: Image Height should be integer.");
            //    Console.WriteLine("\nPress any key to exit the application.");
            //    Console.ReadLine();
            //    return;
            //}

            //if (args.Length > 8)
            //{
            //    if (args.Length < 14)
            //    {
            //        Console.WriteLine("\nError: All three Red, Green and Blue Thresholds should be passed.");
            //        Console.WriteLine("\nPress any key to exit the application.");
            //        Console.ReadLine();
            //        return;
            //    }
            //    else
            //    {
            //        if (!args[8].Equals("-red") || !(int.TryParse(args[9], out redThreshold)) || redThreshold < 0 || redThreshold > 255)
            //        {
            //            Console.WriteLine("\nError: Red Threshold should be in between 0 and 255.");
            //            Console.WriteLine("\nPress any key to exit the application.");
            //            Console.ReadLine();
            //            return;
            //        }

            //        if (!args[10].Equals("-green") || !(int.TryParse(args[11], out greenThreshold)) || greenThreshold < 0 || greenThreshold > 255)
            //        {
            //            Console.WriteLine("\nError: Green Threshold should be in between 0 and 255.");
            //            Console.WriteLine("\nPress any key to exit the application.");
            //            Console.ReadLine();
            //            return;
            //        }

            //        if (!args[12].Equals("-blue") || !(int.TryParse(args[13], out blueThreshold)) || blueThreshold < 0 || blueThreshold > 255)
            //        {
            //            Console.WriteLine("\nError: Blue Threshold should be in between 0 and 255.");
            //            Console.WriteLine("\nPress any key to exit the application.");
            //            Console.ReadLine();
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //    redThreshold = -1;
            //    greenThreshold = -1;
            //    blueThreshold = -1;
            //}
            #endregion

            BinarizeConfiguration configurationDatas;
            if (!(TryParseConfiguration(args, out configurationDatas, out string errMsg)))
            {
                errMsg = errMsg == null ? null : "\nError: " + errMsg;
                ErrMsgPrinter(errMsg);
                return;
            }


            Console.WriteLine("\nImage Binarization in progress...");

            try
            {
                ImageBinarizerApplication obj = new ImageBinarizerApplication();
                //obj.Binarizer(inputImagePath, outputImagePath, imageWidth, imageHeight, redThreshold, greenThreshold, blueThreshold);
                obj.Binarizer(configurationDatas.InputImagePath, configurationDatas.OutputImagePath,
                                configurationDatas.ImageWidth, configurationDatas.ImageHeight,
                                    configurationDatas.RedThreshold, configurationDatas.GreenThreshold,
                                        configurationDatas.BlueThreshold);
            }
            catch (Exception e)
            {
                Console.WriteLine("Image Binarization failed.\n");
                ErrMsgPrinter($"\nError: {e.Message}");
                return;
            }
            ErrMsgPrinter("\nImage Binarization completed.");
        }

        private static void ErrMsgPrinter(string errMsg = null)
        {
            if (string.IsNullOrEmpty(errMsg) == false)
            {
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
        private static bool TryParseConfiguration(string[] args, out BinarizeConfiguration configurationDatas, out string errMsg)
        {
            var parsingObject = new CommandLineParsing(args);
            configurationDatas = new BinarizeConfiguration();

            //
            // Check if -help is called
            if (parsingObject.Help())
            {
                errMsg = null;
                return false;
            }

            //
            // Check if datas Parsed is correct
            if (!parsingObject.Parsing(out configurationDatas, out errMsg))
            {
                return false;
            }

            //
            //Check if input file is valid
            if (!(File.Exists(configurationDatas.InputImagePath)))
            {
                errMsg = "Input file doesn't exist.";
                return false;
            }

            //
            //Check if output dir is valid
            if (!(Directory.Exists(Path.GetDirectoryName(configurationDatas.OutputImagePath))))
            {
                errMsg = "Output Directory doesn't exist.";
                return false;
            }

            //
            //Check if width or height input is valid
            if (configurationDatas.ImageHeight < 0 || configurationDatas.ImageWidth < 0)
            {
                errMsg = "Height and Width should be larger than 0";
                return false;
            }

            //
            //Check if red threshold is valid
            if ((configurationDatas.RedThreshold != -1 && (configurationDatas.RedThreshold < 0 || configurationDatas.RedThreshold > 255)))
            {
                errMsg = "Red Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if green threshold is valid
            if ((configurationDatas.GreenThreshold != -1 && (configurationDatas.GreenThreshold < 0 || configurationDatas.GreenThreshold > 255)))
            {
                errMsg = "Green Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if blue threshold is valid
            if ((configurationDatas.BlueThreshold != -1 && (configurationDatas.BlueThreshold < 0 || configurationDatas.BlueThreshold > 255)))
            {
                errMsg = "Blue Threshold should be in between 0 and 255.";
                return false;
            }

            errMsg = null;
            return true;
        }
    }
}
// --input-image D:\DAENET\image\old.jpg --output-image D:\DAENET\image\out.txt -width 800 -height 225 -red 100 -green 100 -blue 100