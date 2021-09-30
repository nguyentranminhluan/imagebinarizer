using ImageBinarizerApp.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageBinarizerApp
{
    /// <summary>
    /// This class is use to Parse the arguments of the Application
    /// </summary>
    class CommandLineParsing
    {
        public static List<string> HelpArguments = new List<string> { "-help", "-h", "--h", "--help" };
        private List<string> command;

        /// <summary>
        /// Constructor to pass the arguments
        /// </summary>
        /// <param name="args"></param>
        public CommandLineParsing(string[] args)
        {
            command = args.ToList();
        }

        /// <summary>
        /// Conducting parsing process
        /// </summary>
        /// <param name="Configurations"></param>
        /// <returns></returns>
        public bool Parsing(out BinarizeConfiguration Configurations, out string errMsg)
        {
            Configurations = new BinarizeConfiguration();

            //
            //Check if help or inverse argument was called
            CheckHelp();
            CheckInverse();            

            Dictionary<string, string> switchMappings = MappingCommandLine();
                        
            try
            {
                var builder = new ConfigurationBuilder().AddCommandLine(command.ToArray(), switchMappings);
                var config = builder.Build();
                config.Bind(Configurations);

            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return false;
            }
            //Console.WriteLine(Configurations.Inverse);
            if (!PropertiesValidating(Configurations, out errMsg))
                return false;
            errMsg = null;
            return true;


        }

        /// <summary>
        /// Checking inverse argument
        /// </summary>
        private void CheckInverse()
        {
            bool inverse = false;
            while (command.Contains("-inv"))
            {
                command.Remove("-inv");
                inverse = true;
            }
            if (inverse)
            {
                command.Add("-inv");
                command.Add("true");
            }
        }

        /// <summary>
        /// Checking help argument
        /// </summary>
        private void CheckHelp()
        {
            bool help = false;
            foreach(var arg in HelpArguments)
            {
                while (command.Contains(arg))
                {
                    command.Remove(arg);
                    help = true;
                }
            }
            if (help)
            {
                command.Add("-help");
                command.Add("true");
            }
        }

        private static Dictionary<string, string> MappingCommandLine()
        {
            return new Dictionary<string, string>()
            {
                { "-iip", "inputImagePath"},
                { "--input-image", "inputImagePath"},
                { "-oip", "outputImagePath" },
                { "--output-image", "outputImagePath" },
                { "-iw", "imageWidth" },
                { "-width", "imageWidth" },
                { "-ih", "imageHeight"},
                { "-height", "imageHeight"},
                { "-rt", "redThreshold" },
                { "-red", "redThreshold" },
                { "-gt", "greenThreshold" },
                { "-green", "greenThreshold" },
                { "-bt", "blueThreshold"},
                { "-blue", "blueThreshold"},
                { "-help", "help"},
                { "-inv", "inverse"}
            };
        }

        /// <summary>
        /// Check validation of arguments
        /// </summary>
        /// <param name="Configurations"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool PropertiesValidating(BinarizeConfiguration Configurations, out string errMsg)
        {
            //
            //Check if help is call
            if (Configurations.Help)
            {
                PrintHelp();
                errMsg = null;
                return false;
            }

            //
            //Check if input file is valid
            if (!(File.Exists(Configurations.InputImagePath)))
            {
                errMsg = "Input file doesn't exist.";
                return false;
            }

            //
            //Check if output dir is valid
            if (!(Directory.Exists(Path.GetDirectoryName(Configurations.OutputImagePath))))
            {
                errMsg = "Output Directory doesn't exist.";
                return false;
            }

            //
            //Check if width or height input is valid
            if (Configurations.ImageHeight < 0 || Configurations.ImageWidth < 0)
            {
                errMsg = "Height and Width should be larger than 0";
                return false;
            }

            //
            //Check if red threshold is valid
            if ((Configurations.RedThreshold < -1 || Configurations.RedThreshold > 255))
            {
                errMsg = "Red Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if green threshold is valid
            if ((Configurations.GreenThreshold < -1 || Configurations.GreenThreshold > 255))
            {
                errMsg = "Green Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if blue threshold is valid
            if ((Configurations.BlueThreshold < -1 || Configurations.BlueThreshold > 255))
            {
                errMsg = "Blue Threshold should be in between 0 and 255.";
                return false;
            }
            errMsg = null;
            return true;
        }
        
        /// <summary>
        /// Print help to console
        /// </summary>
        private void PrintHelp()
        {
            Console.WriteLine("\nHelp:");
            Console.WriteLine("\n\t- Input image path: {\"-iip\", \"--input-image\", \"--inputImagePath\"}");
            Console.WriteLine("\t- Output image path: {\"-oip\", \"--output-image\", \"--outputImagePath\"}");
            Console.WriteLine("\t- Image width: {\"-iw\", \"-width\", \"--imageWidth\"}");
            Console.WriteLine("\t- Image height: {\"-ih\", \"-height\", \"--imageHeight\"}");
            Console.WriteLine("\t- Red threshold: {\"-rt\", \"-red\", \"--redThreshold\"}");
            Console.WriteLine("\t- Green threshold: {\"-gt\", \"-green\", \"--greenThreshold\"}");
            Console.WriteLine("\t- Blue threshold: {\"-bt\", \"-blue\", \"--blueThreshold\"}");
            Console.WriteLine("\t- Inverse the contrast: {\"-inv\"}");
            Console.WriteLine("\nInput path and output path are required arguments, where as others can be set automaticaly if not specified.");
            Console.WriteLine("\nAdding \"-inv\" to indicate the optional of inversing the contrast of the binarized picture.");
            Console.WriteLine("\nOthers values need to be larger than 0. If needed, use: \n\t-1 to assign threshold default value. " +
                                                                                        "\n\t 0 to assign width and height default value.");
            Console.WriteLine("\n- Example:");
            Console.WriteLine("\t+ With automatic RGB: \n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
            Console.WriteLine("\n\t+ Only Height need to be specify: \n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -height 32");            
            Console.WriteLine("\n\t+ Passing all arguments without inversing the contrast: " +
                                "\n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 \n\t\t-red 100 -green 100 -blue 100");
            Console.WriteLine("\n\t+ Passing all arguments with contrast inversion: " +
                           "\n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 \n\t\t-red 100 -green 100 -blue 100 -inv");
        }
    }


}
