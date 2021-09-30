using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBinarizerLib
{
    /// <summary>
    /// Object contains binarizer parameters for the Image
    /// </summary>
    public class BinarizerParams
    {
        /// <summary>
        /// image width
        /// </summary>
        public int ImageWidth { get; set; } = 0;

        /// <summary>
        /// image height
        /// </summary>
        public int ImageHeight { get; set; } = 0;

        /// <summary>
        /// Red threshold
        /// </summary>
        public int RedThreshold { get; set; } = -1;

        /// <summary>
        /// Green threshold
        /// </summary>
        public int GreenThreshold { get; set; } = -1;

        /// <summary>
        /// Blue threshold
        /// </summary>
        public int BlueThreshold { get; set; } = -1;

        /// <summary>
        /// Grey threshold
        /// </summary>
        public int GreyThreshold { get; set; } = -1;

        /// <summary>
        /// Inverse the contrast of the image
        /// </summary>
        public bool Inverse { get; set; } = false;

        /// <summary>
        /// Calculate base on grey scale threshold
        /// </summary>
        public bool GreyScale { get; set; } = false;
    }
}
