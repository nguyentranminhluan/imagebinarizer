using Daenet.ImageBinarizerLib;
using Daenet.ImageBinarizerLib.Entities;
using FFMediaToolkit;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace VideoBinarizerTool
{

    public class VideoBinarizer
    {
        private BinarizerParams config { get; set; }
        public string videoName { get; set; }
        public string videoPath { get; set; }

        /// <summary>
        /// This method receive the string path of source video as input, use the package FFMediaToolkit to extract all the 
        /// frames of the video into .png files(Bitmap) in a specified folder.
        /// The frames then go into other private methods in sequences : ToBW() => GetDim() => BWToVid() after finishing binarize the 
        /// source video, the method DeleteFolder() is used to clear temporary data which used for the binarized process.
        /// </summary>
        /// <param name="src">the path to the source video</param>
        /// <param name="config">the configuration of the Video binarizer</param>
        public void VidBinarize( BinarizerParams config)
        {
            this.config = config;
            //
            //Get the video name, path to the video and path of the directory.
            videoName = Path.GetFileName(config.InputImagePath);
            videoPath = Path.GetFullPath(config.InputImagePath);
            string sourcePath = Path.GetDirectoryName(videoPath);
            
            //
            //Create the temporary folder for storing the frames from video and binarized frames
            string framesPath = sourcePath + "\\frames";
            string framesBWPath = sourcePath + "\\framesBW";
            System.IO.Directory.CreateDirectory(framesPath);
            System.IO.Directory.CreateDirectory(framesBWPath);
            
            //Config the path which contain the FFMPEG lib
            FFmpegLoader.FFmpegPath = Path.GetFullPath("..\\..\\..\\..\\CommonFiles\\ffmpeg_lib");

            Console.WriteLine("Reading Video as input.....");
            var file = MediaFile.Open(videoPath);

            //
            //Extract all frames from source video then export them into specified folder.
            Console.WriteLine("Binarizing all Frames....");
            int frameNum = 0;
            while (file.Video.TryGetNextFrame(out var imageData))
            {
                imageData.ToBitmap().Save($"{framesPath}\\{frameNum}.png");
                ToBW($"{framesPath}\\{frameNum}.png", $"{framesBWPath}\\BW{frameNum}.png");
                frameNum++;
            }

            //
            //Get the info of Dimension and Framerate for the output video.
            Console.WriteLine("Getting Video Info....");
            int width, height;
            var duration = file.Info.Duration.TotalSeconds;
            GetDim($"{framesBWPath}\\BW{frameNum - 1}.png", out width, out height);
            int frameRate = Convert.ToInt32(frameNum / duration);

            //Convert all binarized frames to Video.
            Console.WriteLine("Converting to Video......");
            BWToVid(width, height, frameRate, framesBWPath);
            
            //let users decide to keep or delete the frames folders
            DeleteFolders(framesPath, framesBWPath);

        }

        /// <summary>
        /// Binerized the output image into black and white (no grey) image
        /// </summary>
        /// <param name="inputPath">string path to the folder that contain frames </param>
        /// <param name="outputPath">string path to the folder that contain binarized frames</param>
        private void ToBW(string inputPath, string outputPath)
        {
            //
            //scaledown the video to 512 pixels in width if resolution too large.
            int ognWidth = Bitmap.FromFile(inputPath).Width > 512 ? 512 : Bitmap.FromFile(inputPath).Width;
            config.InputImagePath = inputPath;
            config.ImageWidth = ognWidth;
            
            
            var img = new ImageBinarizer(config);
            var k = img.GetArrayBinary();
            //
            // the pixel is white if R-G-B values = 255
            //the pixel is black if R-G-B values = 0
            for (int a = 0; a < k.GetLength(0); a++)
            {
                for (int b = 0; b < k.GetLength(1); b++)
                {
                    k[a, b, 0] = k[a, b, 0] * 255;
                    k[a, b, 1] = k[a, b, 0];
                    k[a, b, 2] = k[a, b, 0];
                }
            }
            Bitmap bitmap1 = k.SetPixelsColors();
            bitmap1.Save(outputPath);
            //bitmap1.Dispose();
        }

        /// <summary>
        /// Get the width and height for video base on the frame.
        /// </summary>
        /// <param name="samplePath">the path to one of the frames png</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void GetDim(string samplePath, out int width, out int height)
        {
            width = System.Drawing.Image.FromFile(samplePath).Width;
            height = System.Drawing.Image.FromFile(samplePath).Height;
        }
        
        /// <summary>
        /// Join all binarized image into Video.
        /// </summary>
        /// <param name="wd"></param>
        /// <param name="ht"></param>
        /// <param name="frameRate">framerate of the output radio</param>
        /// <param name="frameBWPath">Path to the binarized frames</param>
        private void BWToVid(int wd, int ht, int frameRate,string frameBWPath)
        {
            string outputPath = $".\\Binerized_{videoName}";
            string fullPath = Path.GetFullPath(outputPath);
            var settings = new VideoEncoderSettings(width: wd, height: ht, framerate: frameRate, codec: VideoCodec.H264);
            settings.EncoderPreset = EncoderPreset.Fast;
            settings.CRF = 17;
            var file = MediaBuilder.CreateContainer(fullPath).WithVideo(settings).Create();
            //
            //Get the paths of all binarized frames,this loop can be replace with
            //files = Directory.GetFiles(inputBW)
            List<string> files = new List<string>();
            for (int k = 0; k < Directory.GetFiles(frameBWPath).Length; k++)
            {
                files.Add($"{frameBWPath}/BW{k}.png");
            }
            
            //
            //Read all binarized frames(bitmap) and add them together to make an video
            foreach (var inputFile in files)
            {
                var binInputFile = File.ReadAllBytes(inputFile);
                var memInput = new MemoryStream(binInputFile);
                var bitmap = Bitmap.FromStream(memInput) as Bitmap;
                var rect = new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size);
                var bitLock = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
                file.Video.AddFrame(bitmapData);
                bitmap.UnlockBits(bitLock);
                bitmap.Dispose();
            }

            file.Dispose();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine($@"The binarized video can be found at " + fullPath);
            Console.ResetColor();
        }

        /// <summary>
        /// Ask users if would like to delete the temporary frames or keep it for 
        /// investigation. If the user decide to keep the folders, the paths will
        /// then printed in the console.
        /// </summary>
        /// <param name="framesPath"></param>
        /// <param name="framesBWPath"></param>
        private void DeleteFolders(string framesPath, string framesBWPath)
        {
            bool ans;
            bool isValid;
            do
            {
                Console.WriteLine("Delete Temporary Folder(yes/no)?:");
                (ans, isValid) = ParseInput(Console.ReadLine());
                if (!isValid)
                {
                    Console.WriteLine("You can only answer with Yes or No");
                }
            }
            while (!isValid);

            if (ans)
            {
                Console.WriteLine("Delete Folder....");
                Directory.Delete(framesPath, true);
                Directory.Delete(framesBWPath, true);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($@"The extracted frames can be found at " + framesPath);
                Console.WriteLine($@"The binarized frames can be found at " + framesBWPath);
                Console.ResetColor();
            }
        }
        
        /// <summary>
        /// map the input answer with the bool value 
        /// </summary>
        /// <param name="input">the input from keyboard of user</param>
        /// <returns></returns>
        (bool ans, bool isValid) ParseInput(string input) =>
        
            char.ToUpper(input.FirstOrDefault()) switch
            {
                'Y' => (true, true),
                'N' => (false, true),
                _ =>(default, false)
            };
        
 
    }
}
