using ImageBinarizerApp.Entities;
using ImageBinarizerLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ImageBinarizerApp
{
    /// <summary>
    /// Class for Image Binarization Application
    /// </summary>
    class ImageBinarizerApplication
    {

        [Obsolete]
        /// <summary>
        /// Method for Image Binarization
        /// </summary>
        public void Binarizer(String inputImagePath, String outputImagePath, int imageWidth, int imageHeight, int redThreshold, int greenThreshold, int blueThreshold, bool inverse)
        {
            Bitmap bitmap = new Bitmap(inputImagePath);

            Dictionary<String, int> imageParams = new Dictionary<string, int>();
            if(imageWidth > 0)
                imageParams.Add("imageWidth", imageWidth);
            else
                imageParams.Add("imageWidth", bitmap.Width);
            if(imageHeight > 0)
                imageParams.Add("imageHeight", imageHeight);
            else
                imageParams.Add("imageWidth", bitmap.Height);
            imageParams.Add("redThreshold", redThreshold);
            imageParams.Add("greenThreshold", greenThreshold);
            imageParams.Add("blueThreshold", blueThreshold);
            

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

            ImageBinarizer img = new ImageBinarizer(imageParams, inverse);
            double[,,] outputData = img.GetBinary(inputData);

            StringBuilder stringArray = new StringBuilder();
            for (int i = 0; i < outputData.GetLength(0); i++)
            {
                for (int j = 0; j < outputData.GetLength(1); j++)
                {
                    stringArray.Append(outputData[i, j, 0]);
                }
                stringArray.AppendLine();
            }
            using (StreamWriter writer = File.CreateText(outputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }

        /// <summary>
        /// method to call Binarizer
        /// </summary>
        /// <param name="config"></param>
        public void Binarizer(BinarizeConfiguration config)
        {
            Bitmap bitmap = new Bitmap(config.InputImagePath);
            BinarizerParams imageParams = MappingParams(config, bitmap);

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

            ImageBinarizer img = new ImageBinarizer(imageParams);
            double[,,] outputData = img.GetBinary(inputData);

            StringBuilder stringArray = new StringBuilder();
            for (int i = 0; i < outputData.GetLength(0); i++)
            {
                for (int j = 0; j < outputData.GetLength(1); j++)
                {
                    stringArray.Append(outputData[i, j, 0]);
                }
                stringArray.AppendLine();
            }
            using (StreamWriter writer = File.CreateText(config.OutputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }

        /// <summary>
        /// mapping parameters from configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private BinarizerParams MappingParams(BinarizeConfiguration config, Bitmap bitmap)
        {
            BinarizerParams imageParams = new BinarizerParams();

            if (config.ImageWidth > 0)
                imageParams.ImageWidth = config.ImageWidth;
            else
                imageParams.ImageWidth = bitmap.Width;
            if (config.ImageHeight > 0)
                imageParams.ImageHeight = config.ImageHeight;
            else
                imageParams.ImageHeight = bitmap.Height;
            imageParams.RedThreshold = config.RedThreshold;
            imageParams.GreenThreshold = config.GreenThreshold;
            imageParams.BlueThreshold = config.BlueThreshold;
            imageParams.GreyThreshold = config.GreyThreshold;
            imageParams.Inverse = config.Inverse;
            imageParams.GreyScale = config.GreyScale;
            return imageParams;
        }
    }
}
