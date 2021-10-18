using System.IO;
using System.Text;


namespace ImageBinarizerLib
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

        #region Constructor
        /// <summary>
        /// Constructor with text file input
        /// </summary>
        /// <param name="path"></param>
        public CodeCreator(string path)
        {
            logoString = File.ReadAllText(path);
            filePath = ".\\LogoPrinter.cs";
        }

        /// <summary>
        /// Constructor with stringBuilder input and output path
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="path"></param>
        public CodeCreator(StringBuilder logo, string path = ".\\LogoPrinter.cs")
        {
            logoString = logo.ToString();
            filePath = path;
        }
        #endregion

        #region Public method
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
