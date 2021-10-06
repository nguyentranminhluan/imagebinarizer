using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageBinarizerLib
{
    /// <summary>
    /// Get Set pixels data
    /// </summary>
    public class ImagePixelsDataHandler
    {

        /// <summary>
        /// Get Pixels data on Win
        /// </summary>
        /// <param name="bitmapInput"></param>
        /// <returns></returns>
        private protected double[,,] GetPixelsColors(Bitmap bitmapInput)
        {
            double[,,] colorData = new double[bitmapInput.Width, bitmapInput.Height, 3];

            //
            //Check bits depth of Image
            if (Bitmap.GetPixelFormatSize(bitmapInput.PixelFormat) / 8 < 3)
                bitmapInput = new Bitmap(bitmapInput, bitmapInput.Width, bitmapInput.Height);

            //
            //Get image pixels array and stride of image
            byte[] pixels = bitmapInput.GetBytes();
            int stride = bitmapInput.GetStride();

            int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapInput.PixelFormat) / 8;
            int heightInPixels = bitmapInput.Height;
            int widthInBytes = bitmapInput.Width * bytesPerPixel;

            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * stride;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    colorData[x / bytesPerPixel, y, 2] = pixels[currentLine + x];
                    colorData[x / bytesPerPixel, y, 1] = pixels[currentLine + x + 1];
                    colorData[x / bytesPerPixel, y, 0] = pixels[currentLine + x + 2];
                }
            }

            return colorData;
        }

        
        /// <summary>
        /// Set Pixel data in faster way
        /// </summary>
        /// <param name="data"></param>
        private protected Bitmap SetPixelsColors(double[,,] data)
        {
            Bitmap bitmapOutput = new Bitmap(data.GetLength(0), data.GetLength(1), PixelFormat.Format24bppRgb);
            BitmapData bitmapData = bitmapOutput.LockBits(new Rectangle(0, 0, bitmapOutput.Width, bitmapOutput.Height), ImageLockMode.ReadWrite, bitmapOutput.PixelFormat);

            int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapOutput.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * bitmapOutput.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;

            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * bitmapData.Stride;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    // calculate new pixel value
                    pixels[currentLine + x] = (byte)data[x / bytesPerPixel, y, 2];
                    pixels[currentLine + x + 1] = (byte)data[x / bytesPerPixel, y, 1];
                    pixels[currentLine + x + 2] = (byte)data[x / bytesPerPixel, y, 0];
                }
            }

            // copy modified bytes back
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            bitmapOutput.UnlockBits(bitmapData);
            return bitmapOutput;
        }
    }
}