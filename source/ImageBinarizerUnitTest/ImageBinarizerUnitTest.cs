using Daenet.ImageBinarizerLib;
using Daenet.ImageBinarizerLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageBinarizerUnitTest
{
    [TestClass]
    public class ImageBinarizerUnitTest
    {
        /// <summary>
        /// Check if the custom width and height and the output width is the same
        /// </summary>
        [TestMethod]
        public void TestCase1()
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
            var output = img.GetStringBinary();

            //
            //Assert
            Assert.AreEqual(wd, output.Split('\n')[0].Length - 1);
            Assert.AreEqual(hg, output.Split('\n').Length - 1);
        }

        ///<summary>
        ///Compare the width and height between input and output without setting up the width and height
        /// </summary>
        [TestMethod]
        public void TestCase2()
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
        /// 
        /// </summary>
        [TestMethod]
        public void Testcase3()
        {

        }
    }
}
