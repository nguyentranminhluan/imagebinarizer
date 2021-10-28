using Daenet.ImageBinarizerLib;
using Daenet.ImageBinarizerLib.Entities;
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
    public class VidHelper
    {
        /// <summary>
        /// Get the width and height for video base on the frame.
        /// </summary>
        /// <param name="samplePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void GetDim(string samplePath, out int width, out int height)
        {
            width = System.Drawing.Image.FromFile(samplePath).Width;
            height = System.Drawing.Image.FromFile(samplePath).Height;
        }

        ///<sumary>
        ///Binerized the output image into black and white (no grey) image
        /// </sumary>
        public void ToBW(string inputPath, string outputPath)
        {
            int ognWidth = Bitmap.FromFile(inputPath).Width > 512 ? 512 : Bitmap.FromFile(inputPath).Width;
            var config = new BinarizerParams()
            {
                InputImagePath = inputPath,
                ImageWidth = ognWidth
            };

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
        public void ToVid(int wd, int ht,int frameRate, string inputBW)
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
                file.Video.AddFrame(bitmapData); // Encode the frame
                bitmap.UnlockBits(bitLock);
                bitmap.Dispose();


            }

            file.Dispose();

            Console.WriteLine($@"The binerized video can be found at " + fullPath);
        }
    }
}
