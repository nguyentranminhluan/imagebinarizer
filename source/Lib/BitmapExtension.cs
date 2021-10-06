﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageBinarizerLib
{

    /// <summary>
    /// Extension for System.Drawing.Bitmap
    /// </summary>
    public static class BitmapExtension
    {

        /// <summary>
        /// Get Bytes array of Image
        /// </summary>
        /// <param name="bitmapInput"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this Bitmap bitmapInput)
        {
            BitmapData bitmapData = bitmapInput.LockBits(new Rectangle(0, 0, bitmapInput.Width, bitmapInput.Height), ImageLockMode.ReadOnly, bitmapInput.PixelFormat);
            int byteCount = bitmapData.Stride * bitmapInput.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            bitmapInput.UnlockBits(bitmapData);

            return pixels;
        }

        /// <summary>
        /// Get stride of Image
        /// </summary>
        /// <param name="bitmapInput"></param>
        /// <returns></returns>
        public static int GetStride(this Bitmap bitmapInput)
        {
            BitmapData bitmapData = bitmapInput.LockBits(new Rectangle(0, 0, bitmapInput.Width, bitmapInput.Height), ImageLockMode.ReadOnly, bitmapInput.PixelFormat);
            bitmapInput.UnlockBits(bitmapData);

            return bitmapData.Stride;
        }
    }
}