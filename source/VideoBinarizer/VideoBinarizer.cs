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
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class VideoBinarizer
    {
        private BinarizerParams config { get; set; }

        public bool DeleteFolder { get; set; }




        /// <summary>
        /// This method receive the string path of source video as input, use the package FFMediaToolkit to extract all the 
        /// frames of the video into .png files(Bitmap) in a specified folder. The method then binerize all of them into
        /// black and white(no grey) using 
        /// </summary>
        /// <param name="src">the path to the source video</param>
        /// <param name="config">the configuration of the Video binarizer</param>
        public void VidBinarize(string src, BinarizerParams config)
        {
            //
            //Create the temporary folder for frames storage
            this.config = config;
            string VideoPath = Path.GetFullPath(src);
            string sourcePath = Path.GetDirectoryName(VideoPath);
            string framesPath = sourcePath + "\\frames";
            string framesBWPath = sourcePath + "\\framesBW";
            System.IO.Directory.CreateDirectory(framesPath);
            System.IO.Directory.CreateDirectory(framesBWPath);
            FFmpegLoader.FFmpegPath = Path.GetFullPath("..\\..\\..\\..\\CommonFiles\\ffmpeg_lib");

            Console.WriteLine("Reading Video as input.....");
            var file = MediaFile.Open(VideoPath);

            //
            //Extract all frames from source video then export them into specified folder.
            Console.WriteLine("Binerizing all Frames....");
            int frameNum = 0;
            while (file.Video.TryGetNextFrame(out var imageData))
            {
                imageData.ToBitmap().Save($"{framesPath}\\{frameNum}.png");
                //imageData.ToBitmap().To
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

            //Convert all binerized frames to Video.
            Console.WriteLine("Converting to Video......");
            BWToVid(width, height, frameRate, framesBWPath);

            Console.Write("Delete used folder or not ?(true/false) : ");
            DeleteFolder = Convert.ToBoolean(Console.ReadLine());

            if (DeleteFolder)
            {
                ClearMemory(framesPath, framesBWPath);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($@"The extracted frames can be found at " + framesPath);
                Console.WriteLine($@"The binerized frames can be found at " + framesBWPath);
            }
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

        ///<sumary>
        ///Binerized the output image into black and white (no grey) image
        /// </sumary>
        private void ToBW(string inputPath, string outputPath)
        {
            int ognWidth = Bitmap.FromFile(inputPath).Width > 512 ? 512 : Bitmap.FromFile(inputPath).Width;
            //var config = new BinarizerParams()
            //{
            //    InputImagePath = inputPath,
            //    ImageWidth = ognWidth
            //};
            config.InputImagePath = inputPath;
            config.ImageWidth = ognWidth;

            //Console.WriteLine(Path.GetFullPath(config.OutputImagePath));
            var img = new ImageBinarizer(config);
            var k = img.GetArrayBinary();
            for (int a = 0; a < k.GetLength(0); a++)
            {
                for (int b = 0; b < k.GetLength(1); b++)
                {
                    k[a, b, 0] = k[a, b, 0] * 225;
                    k[a, b, 1] = k[a, b, 0];
                    k[a, b, 2] = k[a, b, 0];
                }
            }
            Bitmap bitmap1 = k.SetPixelsColors();
            bitmap1.Save(outputPath);
            //bitmap1.Dispose();
        }

        /// <summary>
        /// Join all binarized image into Video.
        /// </summary>
        /// <param name="wd"></param>
        /// <param name="ht"></param>
        /// <param name="frameRate"></param>
        /// <param name="inputBW"></param>
        private void BWToVid(int wd, int ht, int frameRate, string inputBW)
        {
            string outputPath = $".\\BinerizedVideo.mp4";
            string fullPath = Path.GetFullPath(outputPath);
            var settings = new VideoEncoderSettings(width: wd, height: ht, framerate: frameRate, codec: VideoCodec.H264);
            settings.EncoderPreset = EncoderPreset.Fast;
            settings.CRF = 17;
            var file = MediaBuilder.CreateContainer(fullPath).WithVideo(settings).Create();
            List<string> files = new List<string>();
            for (int k = 0; k < Directory.GetFiles(inputBW).Length; k++)
            {
                files.Add($"{inputBW}/BW{k}.png");
            }
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
            Console.WriteLine($@"The binerized video can be found at " + fullPath);
            Console.ResetColor();
        }
        
        /// <summary>
        /// Delete two temporary folders after binarizing video
        /// </summary>
        /// <param name="framesPath"></param>
        /// <param name="framesBWPath"></param>
        private void ClearMemory(string framesPath, string framesBWPath)
        {
            //
            //After output the binerized video, delete two folder which used to stored frames
            Console.WriteLine("Delete Folder....");
            Directory.Delete(framesPath, true);
            Directory.Delete(framesBWPath, true);
        }

        //private bool VidExist(string outputPath, string outputName)
        //{
        //    var dir = new DirectoryInfo(outputPath).GetFiles("*.mp4");
        //    foreach (FileInfo vid in dir)
        //    {
        //        if (vid.Name == outputName)
        //            return true;

        //    }
        //        return false;

        //}
    }
}
