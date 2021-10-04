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
        /// Get Pixel data in faster way
        /// </summary>
        /// <param name="getPixelBitmap"></param>
        /// <returns></returns>
        private protected double[,,] GetPixelsColors(Bitmap getPixelBitmap)
        {
            BitmapData bitmapData = getPixelBitmap.LockBits(new Rectangle(0, 0, getPixelBitmap.Width, getPixelBitmap.Height), ImageLockMode.ReadOnly, getPixelBitmap.PixelFormat);

            double[,,] colorData = new double[getPixelBitmap.Width, getPixelBitmap.Height, 3];

            int bytesPerPixel = Bitmap.GetPixelFormatSize(getPixelBitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * getPixelBitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;

            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * bitmapData.Stride;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    colorData[x / bytesPerPixel, y, 2] = pixels[currentLine + x];
                    colorData[x / bytesPerPixel, y, 1] = pixels[currentLine + x + 1];
                    colorData[x / bytesPerPixel, y, 0] = pixels[currentLine + x + 2];
                }
            }

            getPixelBitmap.UnlockBits(bitmapData);
            return colorData;
        }

        /// <summary>
        /// Set Pixel data in faster way
        /// </summary>
        /// <param name="data"></param>
        private protected Bitmap SetPixelsColors(double[,,] data)
        {
            Bitmap processedBitmap = new Bitmap(data.GetLength(0), data.GetLength(1), PixelFormat.Format24bppRgb);
            BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

            int bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * processedBitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
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
            processedBitmap.UnlockBits(bitmapData);
            return processedBitmap;
        }
    }
}