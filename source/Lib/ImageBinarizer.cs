using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            if (configuration.ImageHeight > 0 && configuration.ImageWidth > 0)
                this.m_TargetSize = new Size(configuration.ImageWidth, configuration.ImageHeight);
        }

        ///// <summary>
        ///// constructor with separate parameters.
        ///// </summary>
        ///// <param name="imageParams"></param>
        ///// <param name="inverse"></param>
        //public ImageBinarizer(Dictionary<String, int> imageParams, bool inverse)
        //{
        //    int targetWidth = 0;
        //    int targetHeight = 0;

        //    if (imageParams.TryGetValue("redThreshold", out int rt))
        //        this.m_RedThreshold = rt;

        //    if (imageParams.TryGetValue("greenThreshold", out int gt))
        //        this.m_GreenThreshold = gt;

        //    if (imageParams.TryGetValue("blueThreshold", out int bt))
        //        this.m_BlueThreshold = bt;

        //    if (imageParams.TryGetValue("imageWidth", out int iw))
        //        targetWidth = iw;

        //    if (imageParams.TryGetValue("imageHeight", out int ih))
        //        targetHeight = ih;
        //    if (inverse)
        //    {
        //        this.m_white = 0;
        //        this.m_black = 1;
        //    }

        //    if (targetHeight > 0 && targetWidth > 0)
        //        this.m_TargetSize = new Size(targetWidth, targetHeight);
        //}

        /// <summary>
        /// constructor with object parameters.
        /// </summary>
        /// <param name="configuration"></param>
        //public void Binarize(BinarizerParams imageParams)
        //{
        //    this.m_RedThreshold = configuration.RedThreshold;
        //    this.m_GreenThreshold = configuration.GreenThreshold;
        //    this.m_BlueThreshold = configuration.BlueThreshold;
        //    this.m_GreyThreshold = configuration.GreyThreshold;

        //    if (configuration.ImageHeight > 0 && configuration.ImageWidth > 0)
        //        this.m_TargetSize = new Size(configuration.ImageWidth, configuration.ImageHeight);

        //    if (configuration.Inverse)
        //    {
        //        this.m_white = 0;
        //        this.m_black = 1;
        //    }

        //    this.m_GreyScale = configuration.GreyScale;

        //}

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
        public double[,,] GetBinary(double[,,] data)
        {


            Bitmap img = new Bitmap(data.GetLength(0), data.GetLength(1));

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    int r = (int)data[i, j, 0];
                    int g = (int)data[i, j, 1];
                    int b = (int)data[i, j, 2];

                    //set limits,bytes can hold values from 0 upto 255
                    img.SetPixel(i, j, Color.FromArgb(255, r, g, b));
                }
            }

            if (this.m_TargetSize != null)
                img = new Bitmap(img, this.m_TargetSize.Value);

            //int hg = img.Height;
            //int wg = img.Width;

            //double[,,] outArray = new double[hg, wg, 3];

            //The average is calculated taking the parameters.When no thresholds are given it automatically calculates the average.            
            CalcAverageRGB(img);

            if (!this.configuration.GreyScale)
            {
                return RGBBinarize(img);
            }
            return GreyScaleBinarize(img);
        }

        /// <summary>
        /// method to call Binarizer
        /// </summary>
        public void RunBinarizer()
        {
            Bitmap bitmap = new Bitmap(this.configuration.InputImagePath);

            int imgWidth = bitmap.Width;
            int imgHeight = bitmap.Height;
            double[,,] inputData = new double[imgWidth, imgHeight, 3];

            for (int i = 0; i < imgWidth; i++)
            {
                for (int j = 0; j < imgHeight; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    inputData[i, j, 0] = color.R;
                    inputData[i, j, 1] = color.G;
                    inputData[i, j, 2] = color.B;
                }
            }

            double[,,] outputData = GetBinary(inputData);

            StringBuilder stringArray = CreateTextFromBinary(outputData);
            using (StreamWriter writer = File.CreateText(this.configuration.OutputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }

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
        /// Average values calculation 
        /// </summary>
        /// <param name="img"></param>
        private void CalcAverageRGB(Bitmap img)
        {
            int hg = img.Height;
            int wg = img.Width;
            Int64 sumR = 0;
            Int64 sumG = 0;
            Int64 sumB = 0;
            //TODO. buffer to get pixels, divide image to avoid overflow of the type of number
            for (int i = 0; i < hg; i++)
            {
                for (int j = 0; j < wg; j++)
                {
                    sumR += img.GetPixel(j, i).R;
                    sumG += img.GetPixel(j, i).G;
                    sumB += img.GetPixel(j, i).B;
                }
            }
            double avgR = sumR / (hg * wg);
            double avgG = sumG / (hg * wg);
            double avgB = sumB / (hg * wg);
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
        /// <param name="img"></param>
        /// <returns></returns>
        private double[,,] GreyScaleBinarize(Bitmap img)
        {
            int hg = img.Height;
            int wg = img.Width;
            double[,,] outArray = new double[hg, wg, 3];
            for (int i = 0; i < hg; i++)
            {
                for (int j = 0; j < wg; j++)
                {
                    outArray[i, j, 0] = ((0.299 * img.GetPixel(j, i).R + 0.587 * img.GetPixel(j, i).G +
                       0.114 * img.GetPixel(j, i).B) > this.configuration.GreyThreshold) ? this.m_white : this.m_black;
                }
            }
            return outArray;
        }

        /// <summary>
        /// Binarize usign RGB threshold
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private double[,,] RGBBinarize(Bitmap img)
        {
            int hg = img.Height;
            int wg = img.Width;
            double[,,] outArray = new double[hg, wg, 3];
            for (int i = 0; i < hg; i++)
            {
                for (int j = 0; j < wg; j++)
                {
                    outArray[i, j, 0] = (img.GetPixel(j, i).R > this.configuration.RedThreshold &&
                                         img.GetPixel(j, i).G > this.configuration.GreenThreshold &&
                                         img.GetPixel(j, i).B > this.configuration.BlueThreshold) ? this.m_white : this.m_black;
                }
            }
            return outArray;
        }


    }
}
