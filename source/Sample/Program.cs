using Daenet.ImageBinarizerLib;
using Daenet.ImageBinarizerLib.Entities;
using System;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ImageBinarizer imgBin = new ImageBinarizer(new BinarizerParams { 
             InputImagePath = "",
             
            });

            //Sample 1
            //string binImg = imgBin.GetBinary();

            // Sample 2
            imgBin.Run();

        }

        /// <summary>
        /// This sample demonstrates how to create the image binary as a string that can be used in the application.
        /// </summary>
        public static void Sample1()
        { 
        
        }


        /// <summary>
        /// This sample demonstrates how to create the image binary aand save it to a file.
        /// </summary>
        public static void Sample2()
        {

        }

        /// <summary>
        /// This sample demonstrates how to create the image binary of the logo and save it as a C# code that can be used in yout application.
        /// </summary>
        public static void Sample3()
        {

        }
    }
}
