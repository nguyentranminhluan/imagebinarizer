using System;
using System.Collections.Generic;
using System.Text;

namespace Daenet.Binarizer.Entities
{
    /// <summary>
    /// Object contains binarizer parameters used for binarization.
    /// </summary>
    public class BinarizerParams
    {
        #region Public properties
        /// <summary>
        /// Input path of the input image
        /// </summary>
        public string InputImagePath { get; set; } = "";

        /// <summary>
        /// Output path where the binarized image file are save
        /// </summary>
        public string OutputImagePath { get; set; }

        /// <summary>
        /// Custom width for binarization
        /// </summary>
        public int ImageWidth { get; set; } = 0;

        /// <summary>
        /// Custom height for binarization
        /// </summary>
        public int ImageHeight { get; set; } = 0;

        /// <summary>
        /// Red threshold for binarization
        /// </summary>
        public int RedThreshold { get; set; } = -1;

        /// <summary>
        /// Green threshold for binarization
        /// </summary>
        public int GreenThreshold { get; set; } = -1;

        /// <summary>
        /// Blue threshold for binarization
        /// </summary>
        public int BlueThreshold { get; set; } = -1;

        /// <summary>
        /// Grey threshold for binarization
        /// </summary>
        public int GreyThreshold { get; set; } = -1;

        /// <summary>
        /// Determine if Inverse of the image is requested
        /// </summary>
        public bool Inverse { get; set; } = false;

        /// <summary>
        /// Determine if binarization base on grey scale threshold
        /// </summary>
        public bool GreyScale { get; set; } = false;

        /// <summary>
        /// Create .cs file instead of text file for printing Logo
        /// </summary>
        public bool CreateCode { get; set; } = false;

        /// <summary>
        /// Get contour of the image
        /// </summary>
        public bool GetContour { get; set; } = false;
        #endregion
    }

}
