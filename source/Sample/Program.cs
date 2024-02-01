using Daenet.Binarizer;
using Daenet.Binarizer.Entities;
using Daenet.Binarizer.ExtensionMethod;
using Daenet.ImageBinarizerApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using VideoBinarizerTool;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This application shows some examples that can be done when using ImageBinarizerLib");

            // Without customizing parameters, all parameters are set or calculated by default except InputImagePath and OutputImagePath.

            // By default, all thresholds' values are calculated and assigned with the average values of image's primary colors. 
            // Threshold setup example:
            //      + if you want a darker blue to be seen as white, you can set BlueThreshold to lower value ie. 50
            //      + if you want only brighter blue to be seen as whith, you can set BlueThreshold to higher value ie. 200

            // GreyThreshold is only be used when GreyScale is set to true.

            // These parameters: ImageWidth, ImageHeight, RedThreshold, GreenThreshold, BlueThreshold, Inverse, GetContour can be used together as custom setting for Binarizing.
            // These parameters: ImageWidth, ImageHeight, GreyScale, GreyThreshold, Inverse, GetContour can be used together as custom setting for Binarizing.
            // CreateCode can be combined with above parameters for creating C# code after binarizing image with custom settup.

            // When assign CreateCode to true, the OutputImagePath parameter does not need to be provided.



            // Uncomment one of these lines of code to run sample
            //Sample1();
            //Sample2();
            //Sample3();
            //Sample4();
            //Sample5();
            //Sample6();
            //Sample7();
            //Sample8();
            //Sample9();
            //Sample10();
            //Sample11();
            //Sample12();
            //Sample14();
            //Sample15();
            //Sample16();
            //Sample17();
            RunAll($@"D:\test for heckler and koch\all videos\Trimmed", $@"D:\test for heckler and koch\all videos\Trimmed\Processed");

            //5241, 5243, 5288



            //VideoToImage("D:\\gow\\OneDrive_1_07-11-2023\\Squeeze Connector 50252259 (Quetschverbinder 59)\\IMG_5260.MOV", 480, 240, 10);
            //MoveImageToFolder("D:\\gow\\OneDrive_1_07-11-2023\\Squeeze Connector 50252259 (Quetschverbinder 59)\\frames", "D:\\gow\\OneDrive_1_07-11-2023\\Squeeze Connector 50252259 (Quetschverbinder 59)\\New folder");

            //This method use VideoToImage method to go through all video in a folder and apple the method to each of them 

            //VideoToImageFolder("D:\\gow\\OneDrive_1_07-11-2023\\Crimp Connector 50252411 (Quetschverbinder 11)", 480, 240, 10);


        }

        public static void MoveImageToFolder(string folderToMove, string targetFolder)
        {
            //this method move images from one folder to the destination folder, if image namge is exist, change the name of the image and move
            string[] files = Directory.GetFiles(folderToMove, "*.png");
            foreach (string sourceFilePath in files)
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(targetFolder, fileName);

                // Check if a file with the same name already exists in the destination folder
                int count = 1;
                while (File.Exists(destinationFilePath))
                {
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string newFileName = $"{nameWithoutExtension}_{count}{fileExtension}";
                    destinationFilePath = Path.Combine(targetFolder, newFileName);
                    count++;
                }

                // Move the file to the destination folder with the unique name
                File.Move(sourceFilePath, destinationFilePath);
            }
        }


        private static void VideoToImageFolder(string v1, int v2, int v3, int v4)
        {
            //this method go through all video in the folder and apply VideoToImage for each video 
            string[] files = Directory.GetFiles(v1, "*.MOV");
            foreach (string file in files)
            {
                VideoToImage(file, v2, v3, v4);
            }
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary as a string that can be used in the application. 
        /// Some arguments will be set in this samples for demo.
        /// </summary>
        public static void Sample1()
        {
            Console.WriteLine("Create the image binary as a string that can be used in the application with preset arguments.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                ImageWidth = 100,
                Inverse = true,
                GetContour = true
            };

            var img = new ImageBinarizer(config);
            Console.WriteLine(img.GetStringBinary());

        }


        /// <summary>
        /// This sample demonstrates how to create the image binary and save it to a text file with default setting.
        /// </summary>
        public static void Sample2()
        {
            Console.WriteLine("Create the image binary and save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt"
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the inverse image binary then save it to a text file.
        /// </summary>
        public static void Sample3()
        {
            Console.WriteLine("Binarized and inversed image then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                Inverse = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the inverse image binary with grey scale then save it to a text file.
        /// </summary>
        public static void Sample4()
        {
            Console.WriteLine("Binarized and inversed image then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                Inverse = true,
                GreyScale = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary with custom width and height then save it to a text file.
        /// </summary>
        public static void Sample5()
        {
            Console.WriteLine("Create the image binary with custom width and height and save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                ImageWidth = 120,
                ImageHeight = 50
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary with custom width, height and custom RGB thresholds then save it to a text file.
        /// </summary>
        public static void Sample6()
        {
            Console.WriteLine("Create the image binary  with custom width, height and custom RGB thresholds then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                ImageWidth = 120,
                ImageHeight = 50,
                RedThreshold = 100,
                BlueThreshold = 80,
                GreenThreshold = 200
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary with grey scale then save it to a text file.
        /// </summary>
        public static void Sample7()
        {
            Console.WriteLine("Binarized image with grey scale then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                GreyScale = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary with grey scale custom grey threshold then save it to a text file.
        /// </summary>
        public static void Sample8()
        {
            Console.WriteLine("Binarized image with grey scale and custom grey threshold then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                GreyScale = true,
                GreyThreshold = 150
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary of the logo with default width is 70 to fit to console window then save it as a C# code that can be used in your application.
        /// </summary>
        public static void Sample9()
        {
            Console.WriteLine("Binarized image of the logo with default width is 70 to fit to console window then save it as a C# code that can be used in your application.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                CreateCode = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to create the image binary of the logo with custom width then save it as a C# code that can be used in your application.
        /// </summary>
        public static void Sample10()
        {
            Console.WriteLine("Binarized image of the logo with with custom width then save it as a C# code that can be used in your application.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                ImageWidth = 100,
                CreateCode = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to get contour after binarizing image with inversion then save it to a text file.
        /// </summary>
        public static void Sample11()
        {
            Console.WriteLine("Create the image binary that show contour with custom width, height and custom RGB thresholds then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                Inverse = true,
                GetContour = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }

        /// <summary>
        /// This sample demonstrates how to get contour after binarizing image with custom width, height and custom RGB thresholds then save it to a text file.
        /// </summary>
        public static void Sample12()
        {
            Console.WriteLine("Create the image binary that show contour with custom width, height and custom RGB thresholds then save it to a text file.");
            var config = new BinarizerParams
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\daenet.png",
                OutputImagePath = ".\\daenet.txt",
                ImageWidth = 120,
                ImageHeight = 50,
                RedThreshold = 100,
                BlueThreshold = 80,
                GreenThreshold = 200,
                GetContour = true
            };

            var img = new ImageBinarizer(config);
            img.Run();
        }


        /// <summary>
        /// Create image binary for 2 version of the same image black&amp;white and normal.
        /// </summary>
        public static void Sample14()
        {
            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\imgblack.jpg",
                OutputImagePath = ".\\imgblack.txt",
                ImageWidth = 1024
            };

            var img = new ImageBinarizer(config);
            img.Run();
            config.InputImagePath = "..\\..\\..\\..\\CommonFiles\\imgnormal.jpg";
            config.OutputImagePath = ".\\imgnormal.txt";
            img.Run();

        }

        /// <summary>
        /// This sample scale down the picture if the width is larger than 1024 to fit in the limit character per line of notepad app on window
        /// </summary>
        public static void Sample15()
        {
            var config = new BinarizerParams()
            {
                InputImagePath = "..\\..\\..\\..\\CommonFiles\\imgnormal.jpg",
                OutputImagePath = ".\\imgnormal.txt",
                ImageWidth = Bitmap.FromFile("..\\..\\..\\..\\CommonFiles\\imgnormal.jpg").Width > 1024 ?
                                                        1024 : Bitmap.FromFile("..\\..\\..\\..\\CommonFiles\\imgnormal.jpg").Width
            };

            Console.WriteLine(Path.GetFullPath(config.OutputImagePath));
            var img = new ImageBinarizer(config);
            img.Run();
        }



        /// <summary>
        ///The problem of txt file is that the space between each line can only be edit in the txt reader. By app default,
        ///there will be gap between each line, which make the ratio of height and width seem different from the source img.
        ///With html file, the gap between each line can be modify to make the preview ratio remain unchanged. This help the demonstration of the app
        ///look better.
        /// </summary>
        public static void Sample16()
        {
            string inputPath = "..\\..\\..\\..\\CommonFiles\\B&W2.jpg";
            Console.WriteLine("Create a image binary with a black and white image with custom width and height and default RGB thresholds then save it to the text file");
            int ognWidth = Bitmap.FromFile(inputPath).Width > 512 ? 512 : Bitmap.FromFile(inputPath).Width;
            Console.WriteLine(ognWidth);

            var config = new BinarizerParams()
            {
                InputImagePath = inputPath,
                OutputImagePath = ".\\imgnormal.html",
                ImageWidth = ognWidth
            };

            Console.WriteLine(Path.GetFullPath(config.OutputImagePath));
            var img = new ImageBinarizer(config);
            var stringHtml = img.GetStringBinary().ToString();
            string quote = "\"";
            string htmlWrite = $@"<!DOCTYPE html> 
<html>
<header>
<title>ImgBiResult</title>
</header>
<body>
<p style={quote}font-family: monospace; font-size: 28px; line-height:14px{quote}>
{stringHtml}
</p>
</body>
</html>
";

            using (StreamWriter writer = File.CreateText(config.OutputImagePath))
            {
                writer.Write(htmlWrite);
            }
            //
            //Open the ouput html file on default web browse.
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = ".\\imgnormal.html",
                UseShellExecute = true
            });
        }

        ///<summary>
        ///This sample read an video of Pedestrians as input value, extract all frames from it, then binerize them and convert them back to video. 
        /// </summary>
        public static void Sample17()
        {

            var vidToBinerize = new VideoBinarizer();
            var config = new BinarizerParams()
            {
                InputImagePath = "L:\\TrainingSet12.09.22\\Schraubendreher\\VID_20220912_123445.mp4",
                //RedThreshold = 100,
                //BlueThreshold = 80,
                //GreenThreshold = 200,
                //GetContour = true

            };
            vidToBinerize.VidBinarize(config);
        }

        /// <summary>
        /// Convert video to Images for training
        /// </summary>
        public static void VideoToImage(string videoPath, int outputWidth, int outputHeight, int frameFilter)
        {
            var vidToImage = new VideoToImages();
            var config = new BinarizerParams()
            {
                ImageHeight = outputHeight,
                ImageWidth = outputWidth,
                FrameFilter = frameFilter,
                InputImagePath = videoPath
            };
            vidToImage.VidToImage(config);
        }


        public static void Sample25()
        {
            //var vidToBinerize = new VideoBinarizer();
            var objectDetector = new ObjectDetector($@"D:\test for heckler and koch\videotest1.wmv", $@"D:\test for heckler and koch\videoresult1.wmv");

            objectDetector.CutVideo(0.0, 30.0);
            //var config = new BinarizerParams()
            //{
            //    InputImagePath = $@"D:\test for heckler and koch\videotest1.wmv",
            //    //RedThreshold = 100,
            //    //BlueThreshold = 80,
            //    //GreenThreshold = 200,
            //    //GetContour = true

            //};
            //vidToBinerize.VidBinarize(config);
        }

        public static void Sample26(string videoPath, string videoOutputPath)
        {
            var objectDetector = new ObjectDetector(videoPath, videoOutputPath);

            objectDetector.CutVideo(0, 30.0);
        }


        public static void Sample27(string input, string output)
        {
            var opticalFlowDetector = new OpticalFlowDetector();
            opticalFlowDetector.Process(input, output);

        }



        static void RunAll(string folderPath, string outputFolderPath)
        {
            // Replace "your_folder_path" with the actual path of your video folder

            // Get all video files in the folder with specific extensions (you can add more if needed)
            string[] videoFiles = Directory.GetFiles(folderPath, "*.wmv");

            // Process each video file
            //foreach (string videoFile in videoFiles)
            //{
            //    string outputFileName = Path.GetFileNameWithoutExtension(videoFile) + "_processed.wmv";
            //    string outputPath = Path.Combine(outputFolderPath, outputFileName);
            //    // Replace "YourProcessingMethod" with the method you want to apply to each video file
            //    Sample26(Path.GetFullPath(videoFile),outputPath);
            //}
            foreach (var videoFile in videoFiles)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(videoFile) + "_processed.wmv";
                string output = Path.Combine(outputFolderPath, outputFileName);
                Sample27(Path.GetFullPath(videoFile), output);
            }
        }






    }
}
