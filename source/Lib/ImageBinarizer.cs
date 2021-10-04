using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using LearningFoundation;

namespace ImageBinarizerLib
{
    /// <summary>
    /// Main class for the Image Binarizer algorithm using Ipipeline
    /// </summary>
    public class ImageBinarizer : IPipelineModule<double[,,], double[,,]>
    {

        private BinarizerParams configuration;

        private int m_white = 1;
        private int m_black = 0;

        private Size? m_TargetSize;

        /// <summary>
        /// By default constructor without parameter
        /// </summary>
        public ImageBinarizer(BinarizerParams configuration)
        {
            this.configuration = configuration;
            if (this.configuration.Inverse)
            {
                this.m_white = 0;
                this.m_black = 1;
            }
        }

        /// <summary>
        /// Method of Interface Ipipline
        /// </summary>
        /// <param name="data">this is the double data coming from unitest.</param>
        /// <param name="ctx">this define the Interface IContext for Data descriptor</param>
        /// <returns></returns>
        public double[,,] Run(double[,,] data, IContext ctx)
        {
            return GetBinary(data);
        }

        /// <summary>
        /// Gets double array representation of the image.I.E.: 010000111000
        /// </summary>
        /// <params name="img">Image instance. Typically bitmap.</params>
        /// <returns></returns>
        public double[,,] GetBinary(double[,,] data, bool needResize = true)
        {
            if (needResize)
            {
                Bitmap img = SetPixelColorUsingLockbits(data);

                this.m_TargetSize = GetTargetSizeFromConfigOrDefault(data.GetLength(0), data.GetLength(1));
                if (this.m_TargetSize != null)
                    img = new Bitmap(img, this.m_TargetSize.Value);

                double[,,] resizedData = GetPixelColorUsingLockbits(img);
                return GetBinaryWithDataArray(resizedData);
            }               

            return GetBinaryWithDataArray(data);
        }
        
        private double[,,] GetBinaryWithDataArray(double[,,] data)
        {

            //The average is calculated taking the parameters.
            //When no thresholds are given, they will be assigned automatically the average values.            
            CalcAverageRGBGrey(data);

            if (!this.configuration.GreyScale)
            {
                return RGBBinarize(data);
            }

            return GreyScaleBinarize(data);
        }
        
        /// <summary>
        /// Average values calculation 
        /// </summary>
        /// <param name="data"></param>
        private void CalcAverageRGBGrey(double[,,] data)
        {
            int hg = data.GetLength(1);
            int wg = data.GetLength(0);

            const int constWidth = 4000;
            const int constHeight = 4000;

            double[,] sumR = new double[wg / constWidth + 1, hg / constHeight + 1];
            double[,] sumG = new double[wg / constWidth + 1, hg / constHeight + 1];
            double[,] sumB = new double[wg / constWidth + 1, hg / constHeight + 1];
            for (int i = 0; i < hg; i++)
            {
                for (int j = 0; j < wg; j++)
                {
                    sumR[j / constWidth, i / constHeight] += data[j, i, 0];
                    sumG[j / constWidth, i / constHeight] += data[j, i, 1];
                    sumB[j / constWidth, i / constHeight] += data[j, i, 2];
                }
            }
            double avgR = 0;
            double avgG = 0;
            double avgB = 0;
            for (int i = 0; i < hg / constHeight + 1; i++)
            {
                for (int j = 0; j < wg / constWidth + 1; j++)
                {
                    avgR += sumR[j, i] / (hg * wg);
                    avgG += sumG[j, i] / (hg * wg);
                    avgB += sumB[j, i] / (hg * wg);
                }
            }
            double avgGrey = 0.299 * avgR + 0.587 * avgG + 0.114 * avgB;//using the NTSC formula            

            if (this.configuration.RedThreshold < 0 || this.configuration.RedThreshold > 255)
                this.configuration.RedThreshold = (int)avgR;

            if (this.configuration.GreenThreshold < 0 || this.configuration.GreenThreshold > 255)
                this.configuration.GreenThreshold = (int)avgG;

            if (this.configuration.BlueThreshold < 0 || this.configuration.BlueThreshold > 255)
                this.configuration.BlueThreshold = (int)avgB;

            if (this.configuration.GreyThreshold < 0 || this.configuration.GreyThreshold > 255)
                this.configuration.GreyThreshold = (int)avgGrey;
        }

        /// <summary>
        /// Binarize using grey scale threshold
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double[,,] GreyScaleBinarize(double[,,] data)
        {
            int hg = data.GetLength(1);
            int wg = data.GetLength(0);
            double[,,] outArray = new double[hg, wg, 3];

            for (int i = 0; i < hg; i++)
            {
                for (int j = 0; j < wg; j++)
                {
                    outArray[i, j, 0] = ((0.299 * data[j, i, 0] + 0.587 * data[j, i, 1] +
                       0.114 * data[j, i, 2]) > this.configuration.GreyThreshold) ? this.m_white : this.m_black;
                }
            }

            return outArray;
        }
        
        /// <summary>
        /// Binarize usign RGB threshold
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double[,,] RGBBinarize(double[,,] data)
        {
            int hg = data.GetLength(1);
            int wg = data.GetLength(0);
            double[,,] outArray = new double[hg, wg, 3];

            for (int i = 0; i < hg; i++)
            {
                for (int j = 0; j < wg; j++)
                {
                    outArray[i, j, 0] = (data[j, i, 0] > this.configuration.RedThreshold &&
                                         data[j, i, 1] > this.configuration.GreenThreshold &&
                                         data[j, i, 2] > this.configuration.BlueThreshold) ? this.m_white : this.m_black;
                }
            }

            return outArray;
        }

        /// <summary>
        /// method to call Binarizer on window
        /// </summary>
        public void RunBinarizerOnWin()
        {
            Bitmap bitmap = new Bitmap(this.configuration.InputImagePath);

            int imgWidth = bitmap.Width;
            int imgHeight = bitmap.Height;

            this.m_TargetSize = GetTargetSizeFromConfigOrDefault(imgWidth, imgHeight);
            if (this.m_TargetSize != null)
            {
                bitmap = new Bitmap(bitmap, this.m_TargetSize.Value);                
            }

            double[,,] inputData = GetPixelColorUsingLockbits(bitmap);

            double[,,] outputData = GetBinary(inputData, false);

            StringBuilder stringArray = CreateTextFromBinary(outputData);
            using (StreamWriter writer = File.CreateText(this.configuration.OutputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }

        /// <summary>
        /// create string array from output
        /// </summary>
        /// <param name="outputData"></param>
        /// <returns></returns>
        private static StringBuilder CreateTextFromBinary(double[,,] outputData)
        {
            StringBuilder stringArray = new StringBuilder();

            for (int i = 0; i < outputData.GetLength(0); i++)
            {
                for (int j = 0; j < outputData.GetLength(1); j++)
                {
                    stringArray.Append(outputData[i, j, 0]);
                }
                stringArray.AppendLine();
            }

            return stringArray;
        }

        /// <summary>
        /// Get size of binarized image
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Size? GetTargetSizeFromConfigOrDefault(int width, int height)
        {
            if (this.configuration.ImageHeight > 0 && this.configuration.ImageWidth > 0)
                return new Size(this.configuration.ImageWidth, this.configuration.ImageHeight);

            if (width == 0 || height == 0)
                return null;

            double ratio = (double)height / width;
            int defaultWidth = 1200;

            if (this.configuration.ImageHeight > 0)
                return new Size((int)(this.configuration.ImageHeight * 2 / ratio), this.configuration.ImageHeight);

            if (this.configuration.ImageWidth > 0)
                return new Size(this.configuration.ImageWidth, (int)(this.configuration.ImageWidth * ratio / 2));

            if (defaultWidth > width)
                return new Size(width, height / 2);

            return new Size(defaultWidth, (int)(defaultWidth * ratio / 2));
        }

        /// <summary>
        /// Get Pixel data in faster way
        /// </summary>
        /// <param name="getPixelBitmap"></param>
        /// <returns></returns>
        private double[,,] GetPixelColorUsingLockbits(Bitmap getPixelBitmap)
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
        private Bitmap SetPixelColorUsingLockbits(double[,,] data)
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
