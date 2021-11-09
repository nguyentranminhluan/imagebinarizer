using FFMediaToolkit.Graphics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VideoBinarizerTool
{
    /// <summary>
    /// this extension method is required by the FFMediaToolkit package to get the frames
    /// from video in .Net framework 
    /// </summary>
    public static class ExtensionClass
    {
        public static unsafe Bitmap ToBitmap(this ImageData bitmap)
        {
            fixed (byte* p = bitmap.Data)
            {
                return new Bitmap(bitmap.ImageSize.Width, bitmap.ImageSize.Height, bitmap.Stride, PixelFormat.Format24bppRgb, new IntPtr(p));
            }
        }
        
        
        /// <summary>
        /// Set Pixel data to System.Drawing.Bitmap object
        /// </summary>
        /// <param name="data">3D array of image data</param>
        /// <returns>System.Drawing.Bitmap object</returns>
        public static Bitmap SetPixelsColors(this double[,,] data)
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
                    pixels[currentLine + x+2] = (byte)data[x / bytesPerPixel, y, 0];
                }
            }

            // copy modified bytes back
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            bitmapOutput.UnlockBits(bitmapData);
            return bitmapOutput;
        }
    }
}
