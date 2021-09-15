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

        /// <summary>
        /// Method for Image Binarization
        /// </summary>
        public void Binarizer(String inputImagePath, String outputImagePath, int imageWidth, int imageHeight, int redThreshold, int greenThreshold, int blueThreshold)
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
            using (StreamWriter writer = File.CreateText(outputImagePath))
            {
                writer.Write(stringArray.ToString());
            }
        }
    }
}
