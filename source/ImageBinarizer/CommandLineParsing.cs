using Daenet.ImageBinarizerApp.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Daenet.ImageBinarizerApp
{
    /// <summary>
    /// This class is use to Parse the arguments of the Application
    /// </summary>
    public class CommandLineParsing
    {
        #region Private members
        public readonly static List<string> HelpArguments = new List<string> { "-h", "--help" };
        private readonly List<string> inverseArguments = new List<string> { "-inv", "--inverse" };
        private readonly List<string> greyScaleArguments = new List<string> { "-gs", "--greyscale" };
        private readonly List<string> createCodeArguments = new List<string> { "-cc", "--createcode", "--create-code" };
        private readonly List<string> getContourArguments = new List<string> { "-gc", "--getcontour" };

        private List<string> command;        
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor to pass the arguments
        /// </summary>
        /// <param name="args">Input arguments</param>
        public CommandLineParsing(string[] args)
        {
            command = args.ToList();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Conducting parsing process that map the input argument to ImageBinarizerApp.Entities.BinarizerConfiguration object.
        /// </summary>
        /// <param name="config">assign output BinarizerConfiguration object to this variable.</param>
        /// <param name="errMsg">assign output error message to this variable.</param>
        /// <returns>True if no error and false if error exit or help argument called.</returns>
        public bool Parse(out BinarizerConfiguration config, out string errMsg)
        {
            config = new BinarizerConfiguration();

            CorrectArgsIfRequired();

            Dictionary<string, string> switchMappings = GetCommandLineMap();

            try
            {
                var builder = new ConfigurationBuilder().AddCommandLine(command.ToArray(), switchMappings);
                var configRoot = builder.Build();
                configRoot.Bind(config);

            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return false;
            }
            //Console.WriteLine(Configurations.Inverse);
            if (!ValidateArgs(config, out errMsg))
                return false;
            errMsg = null;
            return true;


        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get Dictionary for mapping command line
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetCommandLineMap()
        {
            return new Dictionary<string, string>()
            {
                { "-ip", "inputImagePath"},
                { "--input-image", "inputImagePath"},
                { "-op", "outputImagePath" },
                { "--output-image", "outputImagePath" },
                { "-iw", "imageWidth" },
                { "-ih", "imageHeight"},
                { "-rt", "redThreshold" },
                { "-gt", "greenThreshold" },
                { "-bt", "blueThreshold"},
                { "-grt", "greyThreshold"},
                { "-h", "help"},
                { "-inv", "inverse"},
                { "-gs", "greyScale"},
                { "-cc", "createCode"},
                { "--create-code", "createCode"},
                { "-gc", "getContour"}
            };
        }

        /// <summary>
        /// Corect the arguments input that received type boolean.
        /// </summary>
        private void CorrectArgsIfRequired()
        {            
            //
            //Check help argument
            CheckAndCorrectArgument(HelpArguments, "-h");

            //
            //Check inverse argument
            CheckAndCorrectArgument(inverseArguments, "-inv");

            //
            //Check greyscale argument
            CheckAndCorrectArgument(greyScaleArguments, "-gs");

            //
            //Check createcode argument
            CheckAndCorrectArgument(createCodeArguments, "-cc");

            //
            //Check getcontour argument
            CheckAndCorrectArgument(getContourArguments, "-gc");

        }

        /// <summary>
        /// Check if boolean argument called.
        /// </summary>
        /// <param name="Arguments">List of the arguments for one condition</param>
        /// <param name="argCommand">The argument command to set the condition to true when user use one of the arguments in the parameter Arguments</param>
        private void CheckAndCorrectArgument(List<string> Arguments, string argCommand)
        {
            bool hasArg = false;
            foreach (var arg in Arguments)
            {
                while (command.Contains(arg))
                {
                    command.Remove(arg);
                    hasArg = true;
                }
            }
            if (hasArg)
            {
                command.Add(argCommand);
                command.Add("true");
            }
        }        

        /// <summary>
        /// Check validation of arguments. The method take ImageBinarizerApp.Entities.BinarizerConfiguration object 
        /// as input and check if user input arguments are correct.
        /// </summary>
        /// <param name="Configuration">Configuration for binarization</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns></returns>
        private bool ValidateArgs(BinarizerConfiguration Configuration, out string errMsg)
        {
            //
            //Check if help is call
            if (Configuration.Help)
            {
                PrintHelp();
                errMsg = null;
                return false;
            }

            //
            //Check if input file is valid
            if (!(File.Exists(Configuration.InputImagePath)))
            {
                errMsg = "Input file doesn't exist.";
                return false;
            }

            //
            //Check to set output path when code create is required
            if (Configuration.CreateCode)
            {                
                if (Configuration.OutputImagePath.Equals(""))
                    Configuration.OutputImagePath = ".\\LogoPrinter.cs";
            }

            if (Path.GetDirectoryName(Configuration.OutputImagePath) != String.Empty)
            {
                //
                //Check if output dir is valid
                if (!(Directory.Exists(Path.GetDirectoryName(Configuration.OutputImagePath))))
                {
                    errMsg = "Output Directory doesn't exist.";
                    return false;
                }                
            }            

            //
            //Check if width or height input is valid
            if (Configuration.ImageHeight < 0 || Configuration.ImageWidth < 0)
            {
                errMsg = "Height and Width should be larger than 0";
                return false;
            }

            //
            //Check if red threshold is valid
            if ((Configuration.RedThreshold < -1 || Configuration.RedThreshold > 255))
            {
                errMsg = "Red Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if green threshold is valid
            if ((Configuration.GreenThreshold < -1 || Configuration.GreenThreshold > 255))
            {
                errMsg = "Green Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if blue threshold is valid
            if ((Configuration.BlueThreshold < -1 || Configuration.BlueThreshold > 255))
            {
                errMsg = "Blue Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if grey threshold is valid
            if ((Configuration.GreyThreshold < -1 || Configuration.GreyThreshold > 255))
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
            Console.WriteLine("\n\t- Input image path: {\"-ip\", \"--input-image\", \"--inputImagePath\"}");
            Console.WriteLine("\t- Output image path: {\"-op\", \"--output-image\", \"--outputImagePath\"}");
            Console.WriteLine("\t- Image width: {\"-iw\", \"--imageWidth\"}");
            Console.WriteLine("\t- Image height: {\"-ih\", \"--imageHeight\"}");
            Console.WriteLine("\t- Red threshold: {\"-rt\", \"--redThreshold\"}");
            Console.WriteLine("\t- Green threshold: {\"-gt\", \"--greenThreshold\"}");
            Console.WriteLine("\t- Blue threshold: {\"-bt\", \"--blueThreshold\"}");
            Console.WriteLine("\t- Grey threshold: {\"-grt\", \"--greyThreshold\"}");
            Console.WriteLine("\t- Inverse enable: {\"-inv\", \"--inverse\"}");
            Console.WriteLine("\t- Grey scale enable: {\"-gs\", \"--greyscale\"}");
            Console.WriteLine("\t- Create code enable: {\"-cc\", \"--createcode\", \"--create-code\"}");
            Console.WriteLine("\t- Create code enable: {\"-gc\", \"--getcontour\"}");
            Console.WriteLine("\nInput path and output path are required arguments, where as others can be set automatically if not specified.");
            Console.WriteLine("\nAdding \"-inv\" to indicate the option of inversing the contrast of the binarized picture.");
            Console.WriteLine("\nAdding \"-gs\" to indicate the option of calculating threshold base on grey scale. Using \"-grt\" or " +
                                    "\"--greyThreshold\" along with this to set threshold for grey scale binarizer.");
            Console.WriteLine("\nAdding \"-cc\" to indicate the option of creating code file for printing Logo to console.");
            Console.WriteLine("\nOthers values need to be larger than 0. If needed, use: \n\t-1 to assign threshold default value. " +
                                                                                        "\n\t 0 to assign width and height default value.");
            Console.WriteLine("\n- Example:");
            Console.WriteLine("\t+ Binarize with default arguments: \n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt");
            Console.WriteLine("\t+ With automatic RGB: \n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
            Console.WriteLine("\n\t+ Only Height need to be specify: \n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -ih 32");
            Console.WriteLine("\n\t+ Passing all arguments without inversing the contrast: " +
                                "\n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -iw 32 -ih 32 \n\t\t-rt 100 -gt 100 -bt 100");
            Console.WriteLine("\n\t+ Passing all arguments with inversion: " +
                           "\n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -iw 32 -ih 32 \n\t\t-rt 100 -gt 100 -bt 100 -inv");
            Console.WriteLine("\n\t+ Get contour with inversion: \n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -inv -gc");
            Console.WriteLine("\n\t+ Passing all arguments with get contour: " +
                           "\n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -iw 32 -ih 32 \n\t\t-rt 100 -gt 100 -bt 100 -gc");
            Console.WriteLine("\n\t+ Passing all arguments with grey scale calculation: " +
                          "\n\t\timgbin --input-image c:\\a.png --output-image d:\\out.txt -iw 32 -ih 32 \n\t\t-grt 100 -gs");
            Console.WriteLine("\t+ Create code file with default width of 70: \n\t\timgbin --input-image c:\\a.png --create-code");
            Console.WriteLine("\t+ Create code file with custom width or height: \n\t\timgbin --input-image c:\\a.png -iw 150 --create-code" +
                                "\n\t   or \n\t\timgbin --input-image c:\\a.png -ih 150 --create-code");
        }
        #endregion
    }


}
