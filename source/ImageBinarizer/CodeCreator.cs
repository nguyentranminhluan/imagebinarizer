using System.IO;
using System.Text;


namespace Daenet.Binarizer
{
    /// <summary>
    /// Create LogoCreator.cs file for printing logo to console.
    /// </summary>
    public class CodeCreator
    {
        #region Private members
        private static string filePath;
        private static string logoString;
        private static readonly string quote = "\"";
        private static readonly string at = "@";
        private static readonly string openedBracket = "{";
        private static readonly string closedBracket = "}";

        /// <summary>
        /// Produce the string of code to write to .cs file
        /// </summary>
        /// <returns>String of code</returns>
        private string codeField()
        {
            return $@"using System;

namespace LogoBinarizer
{openedBracket}
    public class LogoPrinter
    {openedBracket}
        private string logo = {at}{quote}
{logoString}{quote};

        /// <summary>
        /// Print Logo to console
        /// </summary>
        public void Print()
        {openedBracket}
            Console.WriteLine(logo);
        {closedBracket}
    {closedBracket}
{closedBracket}";
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that receives text file and output path as inputs, 
        /// then create code file base on the string that built from stringBuilder and save it to the output path
        /// </summary>
        /// <param name="inputPath">Input file .txt to make logo</param>
        /// <param name="outputPath">Path to save code file</param>
        public CodeCreator(string inputPath, string outputPath = ".\\LogoPrinter.cs")
        {
            logoString = File.ReadAllText(inputPath);
            filePath = outputPath;
        }

        /// <summary>
        /// Constructor that receives stringBuilder and output path as inputs, 
        /// then create code file base on the stringfrom stringBuilder and save it to the output path
        /// </summary>
        /// <param name="logo">String logo</param>
        /// <param name="outputPath">Path to save code file</param>
        public CodeCreator(StringBuilder logo, string outputPath = ".\\LogoPrinter.cs")
        {
            logoString = logo.ToString();
            filePath = outputPath;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Method to create code file
        /// </summary>
        public void Create()
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.Write(codeField());
            }
        }
        #endregion
    }
}
