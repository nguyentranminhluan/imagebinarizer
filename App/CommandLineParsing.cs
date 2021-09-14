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
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool Parsing(out BinarizeData datas)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-iip", "inputImagePath"},
                { "--input-image", "inputImagePath"},
                { "-oip", "outputImagePath" },
                { "--output-image", "outputImagePath" },
                { "-iw", "imageWidth" },
                { "-width", "imageWidth" },
                { "-height", "imageHeight"},
                { "-rt", "redThreshold" },
                { "-red", "redThreshold" },
                { "-gt", "greenThreshold" },
                { "-green", "greenThreshold" },
                { "-bt", "blueThreshold"},
                { "-blue", "blueThreshold"}
            };

            var data = new BinarizeData();

            try
            {
                var builder = new ConfigurationBuilder().AddCommandLine(command, switchMappings);
                var config = builder.Build();

                config.Bind(data);
                datas = data;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                datas = data;
                return false;
            }


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
            Console.WriteLine("\nPass the arguments as following:");
            Console.WriteLine("\nExample with automatic RGB:\ndotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32");
            Console.WriteLine("\nExample with explicit RGB:\ndotnet ImageBinarizerApp --input-image c:\\a.png --output-image d:\\out.txt -width 32 -height 32 -red 100 -green 100 -blue 100");
        }
    }

    /// <summary>
    /// Collect requested Binarize data
    /// </summary>
    class BinarizeData
    {
        private String inputImagePath = "";
        private String outputImagePath = "";
        private int imageWidth = 0;
        private int imageHeight = 0;
        private int redThreshold = -1;
        private int greenThreshold = -1;
        private int blueThreshold = -1;
        public string InputImagePath { get { return inputImagePath; } set { inputImagePath = value; } }
        public string OutputImagePath { get { return outputImagePath; } set { outputImagePath = value; } }
        public int ImageWidth { get { return imageWidth; } set { imageWidth = value; } }
        public int ImageHeight { get { return imageHeight; } set { imageHeight = value; } }
        public int RedThreshold { get { return redThreshold; } set { redThreshold = value; } }
        public int GreenThreshold { get { return greenThreshold; } set { greenThreshold = value; } }
        public int BlueThreshold { get { return blueThreshold; } set { blueThreshold = value; } }

    }
}
