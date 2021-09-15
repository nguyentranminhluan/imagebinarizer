using System;
using System.IO;

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
            Console.WriteLine("\nWelcome to Image Binarizer Application [Version 1.0.2]");
            Console.WriteLine("Copyright <c> 2019 daenet GmbH, Damir Dobric. All rights reserved.");
            Console.WriteLine("\nUse following command for help:");
            Console.WriteLine("dotnet ImageBinarizerApp -help");

            //Test if necessary input arguments were supplied.
            /*if (args.length < 8)
            {
                if (args.length == 1 && args[0].equals("-help"))
                {
                    console.writeline("\nhelp:");
                    console.writeline("\npass the arguments as following:");
                    console.writeline("\nexample with automatic rgb:\ndotnet imagebinarizerapp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
                    console.writeline("\nexample with explicit rgb:\ndotnet imagebinarizerapp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 -red 100 -green 100 -blue 100");
                }
                else
                {
                    console.writeline("\nerror: all necessary arguments are not passed. please pass the arguments first.");
                }
                console.writeline("\npress any key to exit the application.");
                console.readline();
                return;
            }*/

            /*String inputImagePath = "";
            String outputImagePath = "";
            int imageWidth = 0;
            int imageHeight = 0;
            int redThreshold = -1;
            int greenThreshold = -1;
            int blueThreshold = -1;*/

            /*if (!(args[0].Equals("--input-image") && File.Exists(args[1])))            
            {
                Console.WriteLine("\nError: Input file doesn't exist.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return;
            }*/


            //inputImagePath = args[1];


            //int separatorIndex = args[3].LastIndexOf(Path.DirectorySeparatorChar);            
            /*if (!(args[2].Equals("--output-image") && separatorIndex >= 0 && Directory.Exists(args[3].Substring(0, separatorIndex))))           
            {
                Console.WriteLine("\nError: Output Directory doesn't exist.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return;
            }*/

            //outputImagePath = args[3];


            /*if (!args[4].Equals("-width") || !int.TryParse(args[5], out imageWidth))
            {
                Console.WriteLine("\nError: Image Width should be integer.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return;
            }

            if (!args[6].Equals("-height") || !int.TryParse(args[7], out imageHeight))
            {
                Console.WriteLine("\nError: Image Height should be integer.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return;
            }*/

            /*if (args.Length > 8)
            {
                if (args.Length < 14)
                {
                    Console.WriteLine("\nError: All three Red, Green and Blue Thresholds should be passed.");
                    Console.WriteLine("\nPress any key to exit the application.");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    if (!args[8].Equals("-red") || !(int.TryParse(args[9], out redThreshold)) || redThreshold < 0 || redThreshold > 255)
                    {
                        Console.WriteLine("\nError: Red Threshold should be in between 0 and 255.");
                        Console.WriteLine("\nPress any key to exit the application.");
                        Console.ReadLine();
                        return;
                    }

                    if (!args[10].Equals("-green") || !(int.TryParse(args[11], out greenThreshold)) || greenThreshold < 0 || greenThreshold > 255)
                    {
                        Console.WriteLine("\nError: Green Threshold should be in between 0 and 255.");
                        Console.WriteLine("\nPress any key to exit the application.");
                        Console.ReadLine();
                        return;
                    }

                    if (!args[12].Equals("-blue") || !(int.TryParse(args[13], out blueThreshold)) || blueThreshold < 0 || blueThreshold > 255)
                    {
                        Console.WriteLine("\nError: Blue Threshold should be in between 0 and 255.");
                        Console.WriteLine("\nPress any key to exit the application.");
                        Console.ReadLine();
                        return;
                    }
                }
            }
            else
            {
                redThreshold = -1;
                greenThreshold = -1;
                blueThreshold = -1;
            }*/
            
            var datas = new BinarizeData();
            if(!(argumentValidation(args, out datas))){
                return;
            }           


            Console.WriteLine("\nImage Binarization in progress...");

            try
            {
                ImageBinarizerApplication obj = new ImageBinarizerApplication();
                //obj.Binarizer(inputImagePath, outputImagePath, imageWidth, imageHeight, redThreshold, greenThreshold, blueThreshold);
                obj.Binarizer(datas.InputImagePath, datas.OutputImagePath, datas.ImageWidth, datas.ImageHeight,
                                datas.RedThreshold, datas.GreenThreshold, datas.BlueThreshold);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nImage Binarization completed.");
            Console.WriteLine("\nPress any key to exit the application.");

            Console.ReadLine();

        }

        /// <summary>
        /// Check validation of arguments
        /// </summary>
        /// <param name="args"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        private static bool argumentValidation(string[] args, out BinarizeData datas)
        {
            var parsingObject = new CommandLineParsing(args);
            datas = new BinarizeData();
            // Check if -help is called
            if (parsingObject.Help())
            {
                parsingObject.PrintHelp();
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }           
            //Check if datas Parsed is correct
            if (!parsingObject.Parsing(out datas))
            {
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            //Check if input file is valid
            if (!(File.Exists(datas.InputImagePath)))
            {
                Console.WriteLine("\nError: Input file doesn't exist.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            //Check if output dir is valid
            if (!(Directory.Exists(Path.GetDirectoryName(datas.OutputImagePath))))
            {
                Console.WriteLine("\nError: Output Directory doesn't exist.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            //Check if width or height input is valid
            if(datas.ImageHeight < 0 || datas.ImageWidth < 0)
            {
                Console.WriteLine("\nError: Height and Width should be larger than 0");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            //Check if red threshold is valid
            if ((datas.RedThreshold != -1 && (datas.RedThreshold < 0 || datas.RedThreshold > 255)))
            {
                Console.WriteLine("\nError: Red Threshold should be in between 0 and 255.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            //Check if green threshold is valid
            if ((datas.GreenThreshold != -1 && (datas.GreenThreshold < 0 || datas.GreenThreshold > 255)))
            {
                Console.WriteLine("\nError: Green Threshold should be in between 0 and 255.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            //Check if blue threshold is valid
            if ((datas.BlueThreshold != -1 && (datas.BlueThreshold < 0 || datas.BlueThreshold > 255)))
            {
                Console.WriteLine("\nError: Green Threshold should be in between 0 and 255.");
                Console.WriteLine("\nPress any key to exit the application.");
                Console.ReadLine();
                return false;
            }
            return true;
        }
    }
}
