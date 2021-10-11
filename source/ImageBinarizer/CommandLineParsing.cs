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
    public class CommandLineParsing
    {
        public static List<string> HelpArguments = new List<string> { "-help", "-h", "--h", "--help" };

        private List<string> command;
        private static Dictionary<string, string> GetCommandLineMap()
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
                { "-grey", "greyThreshold"},
                { "-help", "help"},
                { "-inv", "inverse"},
                { "-gs", "greyScale"}
            };
        }

        #region Public methods
        /// <summary>
        /// Constructor to pass the arguments
        /// </summary>
        /// <param name="args">input arguments</param>
        public CommandLineParsing(string[] args)
        {
            command = args.ToList();
        }

        /// <summary>
        /// Conducting parsing process that map the input argument to ImageBinarizerApp.Entities.BinarizerConfiguration object.
        /// </summary>
        /// <param name="Configurations">assign output BinarizerConfiguration object to this variable.</param>
        /// <param name="errMsg">assign output error message to this variable.</param>
        /// <returns>True if no error and false if error exit or help argument called.</returns>
        public bool Parse(out BinarizerConfiguration Configurations, out string errMsg)
        {
            Configurations = new BinarizerConfiguration();

            CorrectArgsIfRequired();

            Dictionary<string, string> switchMappings = GetCommandLineMap();

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
            if (!ValidateArgs(Configurations, out errMsg))
                return false;
            errMsg = null;
            return true;


        }
        #endregion

        #region Private methods
        /// <summary>
        /// Corect the arguments input that received type boolean.
        /// </summary>
        private void CorrectArgsIfRequired()
        {
            //
            //Check if help, inverse, or Geryscale argument was called
            CheckAndCorrectHelpArgument();
            CheckAndCorrectInverseArgument();
            CheckAndCorrectGreyScaleArgument();
        }

        /// <summary>
        /// Checking GreyScale argument.
        /// </summary>
        private void CheckAndCorrectGreyScaleArgument()
        {
            bool greyScale = false;
            while (command.Contains("-gs"))
            {
                command.Remove("-gs");
                greyScale = true;
            }
            if (greyScale)
            {
                command.Add("-gs");
                command.Add("true");
            }
        }

        /// <summary>
        /// Checking inverse argument.
        /// </summary>
        private void CheckAndCorrectInverseArgument()
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
        /// Checking help argument.
        /// </summary>
        private void CheckAndCorrectHelpArgument()
        {
            bool help = false;
            foreach (var arg in HelpArguments)
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

        /// <summary>
        /// Check validation of arguments. The method take ImageBinarizerApp.Entities.BinarizerConfiguration object as input and check if user input arguments are correct.
        /// </summary>
        /// <param name="Configurations">Configuration for binarization</param>
        /// <param name="errMsg">output error message</param>
        /// <returns></returns>
        private bool ValidateArgs(BinarizerConfiguration Configurations, out string errMsg)
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

            if (Path.GetDirectoryName(Configurations.OutputImagePath) != String.Empty)
            {
                //
                //Check if output dir is valid
                if (!(Directory.Exists(Path.GetDirectoryName(Configurations.OutputImagePath))))
                {
                    errMsg = "Output Directory doesn't exist.";
                    return false;
                }                
            }
            Configurations.OutputImagePath = Path.GetFullPath(Configurations.OutputImagePath);

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

            //
            //Check if grey threshold is valid
            if ((Configurations.GreyThreshold < -1 || Configurations.GreyThreshold > 255))
            {
                errMsg = "Grey Threshold should be in between 0 and 255.";
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
            Console.WriteLine("\t- Grey threshold: {\"-grey\", \"--greyThreshold\"}");
            Console.WriteLine("\t- Inverse the contrast: {\"-inv\"}");
            Console.WriteLine("\t- Grey scale: {\"-grey\"}");
            Console.WriteLine("\nInput path and output path are required arguments, where as others can be set automatically if not specified.");
            Console.WriteLine("\nAdding \"-inv\" to indicate the optional of inversing the contrast of the binarized picture.");
            Console.WriteLine("\nAdding \"-gs\" to indicate the optional of calculate threshold base on grey scale. Using \"-grey\" or " +
                                    "\"--greyThreshold\" along with this to set threshold for grey scale binarizer.");
            Console.WriteLine("\nOthers values need to be larger than 0. If needed, use: \n\t-1 to assign threshold default value. " +
                                                                                        "\n\t 0 to assign width and height default value.");
            Console.WriteLine("\n- Example:");
            Console.WriteLine("\t+ With automatic RGB: \n\t\tdotnet ImageBinarizer --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
            Console.WriteLine("\n\t+ Only Height need to be specify: \n\t\tdotnet ImageBinarizer --input-image c:\\a.png --output-image d:\\out.txt -height 32");
            Console.WriteLine("\n\t+ Passing all arguments without inversing the contrast: " +
                                "\n\t\tdotnet ImageBinarizer --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 \n\t\t-red 100 -green 100 -blue 100");
            Console.WriteLine("\n\t+ Passing all arguments with contrast inversion: " +
                           "\n\t\tdotnet ImageBinarizer --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 \n\t\t-red 100 -green 100 -blue 100 -inv");
            Console.WriteLine("\n\t+ Passing all arguments with grey scale calculation: " +
                          "\n\t\tdotnet ImageBinarizer --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 \n\t\t-grey 100 -gs");
        }
        #endregion
    }


}
