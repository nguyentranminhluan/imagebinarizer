using ImageBinarizerApp.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        private string[] command;

        /// <summary>
        /// Constructor to pass the arguments
        /// </summary>
        /// <param name="args"></param>
        public CommandLineParsing(string[] args)
        {
            command = args;
        }

        /// <summary>
        /// Conducting parsing process
        /// </summary>
        /// <param name="Configurations"></param>
        /// <returns></returns>
        public bool Parsing(out BinarizeConfiguration Configurations)
        {
            Dictionary<string, string> switchMappings = mappingCommandLine();

            Configurations = new BinarizeConfiguration();

            try
            {
                var builder = new ConfigurationBuilder().AddCommandLine(command, switchMappings);
                var config = builder.Build();

                config.Bind(Configurations);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


        }

        private static Dictionary<string, string> mappingCommandLine()
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
                { "-blue", "blueThreshold"}
            };
        }

        /// <summary>
        /// Check if help is call
        /// </summary>
        /// <returns></returns>
        public bool Help()
        {
            string[] helps = { "-help", "-h", "--h" };
            if (command.Length == 1 && helps.Contains(command[0]))
                return true;
            return false;
        }

        /// <summary>
        /// Print help to console
        /// </summary>
        public void PrintHelp()
        {
            Console.WriteLine("\nHelp:");
            Console.WriteLine("\n\t- Input image path: {\"- iip\", \"--input - image\"}");
            Console.WriteLine("\t- Output image path: {\"-oip\", \"--output-image\"}");
            Console.WriteLine("\t- Image width: {\"-iw\", \"-width\"}");
            Console.WriteLine("\t- Image height: {\"-ih\", \"-height\"}");
            Console.WriteLine("\t- Red threshold: {\"-rt\", \"-red\"}");
            Console.WriteLine("\t- Green threshold: {\"-gt\", \"-green\"}");
            Console.WriteLine("\t- Blue threshold: {\"-bt\", \"-blue\"}");
            Console.WriteLine("\nInput path and output path are required arguments, where as others can be set automaticaly if not specified.");
            Console.WriteLine("\nOthers values need to be larger than 0. If needed, use: \n\t-1 to assign threshold default value. \n\t0 to assign width and height default value.");
            Console.WriteLine("\n- Example:");
            Console.WriteLine("\t+ With automatic RGB: \n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
            Console.WriteLine("\n\t+ Only Height need to be specify: \n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -height 32");
            Console.WriteLine("\n\t+ Passing all arguments: \n\t\tdotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 \n\t\t-red 100 -green 100 -blue 100");
        }
    }

    
}
