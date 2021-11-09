using Daenet.ImageBinarizerLib;
using Daenet.ImageBinarizerLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace ImageBinarizerUnitTest
{
    [TestClass]
    public class ImageBinarizerUnitTest
    {
        /// <summary>
        /// Check if the custom width and height is the same as the output string width and height
        /// </summary>
        [TestMethod]
        public void OutputSizeMustBeTheSameAsCustomSize()
        {

            //
            //Prepre the test
            int wd = 320;
            int hg = 240;
            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                ImageWidth = wd,
                ImageHeight = hg

            };
            var img = new ImageBinarizer(config);
            var outputString = img.GetStringBinary();
            var outputArray = img.GetArrayBinary();

            //
            //Is output string size equal
            Assert.AreEqual(wd, outputString.Split('\n')[0].Length - 1);
            Assert.AreEqual(hg, outputString.Split('\n').Length - 1);

            //
            //Is output array size equal
            Assert.AreEqual(wd, outputArray.GetLength(0));
            Assert.AreEqual(hg, outputArray.GetLength(1));
        }

        ///<summary>
        ///Compare the width and height between input and output without setting up the width and height
        /// </summary>
        [TestMethod]
        public void OutputSizeMustBeTheSameAsInputImageSizeByDefault()
        {
            var inputWidth = System.Drawing.Image.FromFile("..\\..\\..\\..\\CommonFiles\\daenet.png").Width;
            var inputHeight = System.Drawing.Image.FromFile("..\\..\\..\\..\\CommonFiles\\daenet.png").Height;
            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt"
            };
            var img = new ImageBinarizer(config);
            var output = img.GetStringBinary();

            Assert.AreEqual(inputHeight, output.Split('\n').Length - 1);
            Assert.AreEqual(inputWidth, output.Split('\n')[0].Length - 1);
        }
        /// <summary>
        /// Test if red threshhold work correctly with custom threshold value 
        /// </summary>
        [TestMethod]
        public void RedThresholdTest()
        {
            //
            //Create a bitmap data that every pixel got the same color(RGB) value.
            Bitmap bmp = new Bitmap(320, 320);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {

                    //redSample.
                    bmp.SetPixel(x, y, Color.FromArgb(255, 200, 2, 2));

                }
            }
            bmp.Save("..\\..\\..\\..\\CommonFiles\\redSample.png");

            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\redSample.png",
                OutputImagePath = ".\\redTest.txt",
                RedThreshold = 199,
                GreenThreshold = 1,
                BlueThreshold = 1,
            };

            var img = new ImageBinarizer(config);
            var output = img.GetStringBinary();

            //As the red value is larger than threshold value every pixel should got value 1
            Assert.IsFalse(output.Contains("0"));

            config.RedThreshold = 201;
            img = new ImageBinarizer(config);
            output = img.GetStringBinary();

            //As the red value is less than threshold value every pixel should got value 0
            Assert.IsFalse(output.Contains("1"));
        }

        /// <summary>
        /// Test if green threshhold work correctly with custom threshold value 
        /// </summary>
        [TestMethod]
        public void GreenThresholdTest()
        {
            //
            //Create a bitmap data that every pixel got the same color(RGB) value.
            Bitmap bmp = new Bitmap(320, 320);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {

                    //greenSample.
                    bmp.SetPixel(x, y, Color.FromArgb(255, 2, 200, 2));

                }
            }
            bmp.Save("..\\..\\..\\..\\CommonFiles\\greenSample.png");

            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\greenSample.png",
                OutputImagePath = ".\\greenTest.txt",
                RedThreshold = 1,
                GreenThreshold = 199,
                BlueThreshold = 1,
            };

            var img = new ImageBinarizer(config);
            var output = img.GetStringBinary();

            //As the green value is larger than threshold value every pixel should got value 1
            Assert.IsFalse(output.Contains("0"));

            config.GreenThreshold = 201;
            img = new ImageBinarizer(config);
            output = img.GetStringBinary();

            //As the green value is less than threshold value every pixel should got value 0
            Assert.IsFalse(output.Contains("1"));

        }

        /// <summary>
        /// Test if blue threshhold work correctly with custom threshold value 
        /// </summary>
        [TestMethod]
        public void BlueThresholdTest()
        {
            //
            //Create a bitmap data that every pixel got the same color(RGB) value.
            Bitmap bmp = new Bitmap(320, 320);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {

                    //blueSample.
                    bmp.SetPixel(x, y, Color.FromArgb(255, 2, 2, 200));

                }
            }
            bmp.Save("..\\..\\..\\..\\CommonFiles\\blueSample.png");

            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\blueSample.png",
                OutputImagePath = ".\\blueTest.txt",
                RedThreshold = 1,
                GreenThreshold = 1,
                BlueThreshold = 199,
            };

            var img = new ImageBinarizer(config);
            var output = img.GetStringBinary();

            //As the blue value is larger than threshold value every pixel should got value 1
            Assert.IsFalse(output.Contains("0"));

            config.BlueThreshold = 201;
            img = new ImageBinarizer(config);
            output = img.GetStringBinary();

            //As the blue value is less than threshold value every pixel should got value 0
            Assert.IsFalse(output.Contains("1"));
        }

        [TestMethod]
        public void GreyThresholdTest()
        {

        }

        /// <summary>
        /// Compare the output between parameter Inverse = true and Inverse = false 
        /// two outputs.
        /// </summary>
        [TestMethod]
        public void IsInverse()
        {
            var configInverse = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                Inverse = true

            };


            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenetInverse.txt",
                Inverse = false
            };
            var img = new ImageBinarizer(config);
            var imgInverse = new ImageBinarizer(configInverse);


            var outputArray = img.GetStringBinary();
            var outputArrayInverse = imgInverse.GetStringBinary();


            for (int i = 0; i < outputArray.Length; i++)
            {
                if (outputArray[i] == '1')
                {
                    Assert.AreEqual('0', outputArrayInverse[i]);
                }
                else if (outputArray[i] == '0')
                {
                    Assert.AreEqual('1', outputArrayInverse[i]);
                }

            }
        }

        [TestMethod]
        public void IsCodeCreated()
        {

        }


    }
}
