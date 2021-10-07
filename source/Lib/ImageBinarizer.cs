using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using LearningFoundation;
using SkiaSharp;

namespace ImageBinarizerLib
{
    /// <summary>
    /// Main class for the Image Binarizer algorithm using Ipipeline
    /// </summary>
    public class ImageBinarizer : ImagePixelsDataHandler, IPipelineModule<double[,,], double[,,]>
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

        #region Public Methods

        /// <summary>
        /// If you use the binarizer inside of the LearningApiPipeline, you should use this method.
        /// </summary>
        /// <param name="data">this is the double data coming from unitest.</param>
        /// <param name="ctx">this define the Interface IContext for Data descriptor</param>
        /// <returns></returns>
        public double[,,] Run(double[,,] data, IContext ctx)
        {
            return GetBinary(ResizeImageData(data));
        }

        /// <summary>
        /// Runs the binarization of the image.
        /// </summary>
        public void Run()
        {
            Bitmap bitmap = new Bitmap(this.configuration.InputImagePath);

            int imgWidth = bitmap.Width;
            int imgHeight = bitmap.Height;

            this.m_TargetSize = GetTargetSizeFromConfigOrDefault(imgWidth, imgHeight);
            if (this.m_TargetSize != null)
            {
                bitmap = new Bitmap(bitmap, this.m_TargetSize.Value);
            }

            double[,,] inputData = GetPixelsColors(bitmap);

            double[,,] outputData = GetBinary(inputData);

            StringBuilder sb = CreateTextFromBinary(outputData);

            using (StreamWriter writer = File.CreateText(this.configuration.OutputImagePath))
            {
                writer.Write(sb.ToString());
            }
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

            double[,,] inputData = GetPixelsColors(bitmap);

            double[,,] outputData = GetBinary(inputData);

            StringBuilder stringArray = CreateTextFromBinary(outputData);
            using (StreamWriter writer = File.CreateText(this.configuration.OutputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }

        /// <summary>
        /// method to call Binarizer on Linux
        /// </summary>
        public void RunBinarizerOnLinux()
        {
            SKBitmap skBitmap = SKBitmap.Decode(this.configuration.InputImagePath);

            int imgWidth = skBitmap.Width;
            int imgHeight = skBitmap.Height;

            this.m_TargetSize = GetTargetSizeFromConfigOrDefault(imgWidth, imgHeight);
            if (this.m_TargetSize != null)
            {
                SKImageInfo info = new SKImageInfo(this.m_TargetSize.Value.Width, this.m_TargetSize.Value.Height, SKColorType.Rgba8888);
                skBitmap = skBitmap.Resize(info, SKFilterQuality.High);
            }

            double[,,] inputData = GetPixelsColors(skBitmap);

            double[,,] outputData = GetBinary(inputData);

            StringBuilder stringArray = CreateTextFromBinary(outputData);
            using (StreamWriter writer = File.CreateText(this.configuration.OutputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }

        #endregion


        private double[,,] ResizeImageData(double[,,] data)
        {
            Bitmap img = SetPixelsColors(data);

            this.m_TargetSize = GetTargetSizeFromConfigOrDefault(data.GetLength(0), data.GetLength(1));

            if (this.m_TargetSize != null)
                img = new Bitmap(img, this.m_TargetSize.Value);

            double[,,] resizedData = GetPixelsColors(img);

            data = resizedData;
            return data;
        }

        /// <summary>
        /// Get Binary array with input array double
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double[,,] GetBinary(double[,,] data)
        {

            // The average is calculated taking the parameters.
            // When no thresholds are given, they will be assigned automatically the average values.            
            CalcAverageRGBGrey(data);

            if (!this.configuration.GreyScale)
            {
                return RgbScaleBinarize(data);
            }

            return GreyScaleBinarize(data);
        }

        /// <summary>
        /// Average values calculation 
        /// </summary>
        /// <param name="data">xz\vxxvcc</param>
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
        private double[,,] RgbScaleBinarize(double[,,] data)
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
                return new Size((int)(this.configuration.ImageHeight / ratio), this.configuration.ImageHeight);

            if (this.configuration.ImageWidth > 0)
                return new Size(this.configuration.ImageWidth, (int)(this.configuration.ImageWidth * ratio));

            if (defaultWidth > width)
                return new Size(width, height);

            return new Size(defaultWidth, (int)(defaultWidth * ratio));
        }
    }
}
