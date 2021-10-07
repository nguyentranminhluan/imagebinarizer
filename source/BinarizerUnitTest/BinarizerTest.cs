using ImageBinarizerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinarizerUnitTest
{
    [TestClass]
    public class BinarizerTest
    {
        BinarizerParams config = new BinarizerParams();
        [TestInitialize]
        public void InitParams()
        {
            config.InputImagePath = "D:\\DAENET\\image\\daenet.png";
            config.OutputImagePath = "D:\\DAENET\\image\\out.txt";            
        }


        [TestMethod]
        public void RunBinarizerOnLinuxCheckThreshold()
        {
            ImageBinarizer obj = new ImageBinarizer(config);
            obj.RunBinarizerOnLinux();
            Console.WriteLine(config.RedThreshold);
            Console.WriteLine(config.GreenThreshold);
            Console.WriteLine(config.BlueThreshold);
            Assert.IsTrue(config.BlueThreshold > config.RedThreshold);
        }
    }
}
