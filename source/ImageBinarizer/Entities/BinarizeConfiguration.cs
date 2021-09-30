using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageBinarizerApp.Entities
{
    /// <summary>
    /// Collect requested Binarize data
    /// </summary>
    class BinarizeConfiguration
    {
        public string InputImagePath { get; set; } = "";
        public string OutputImagePath { get; set; } = "";
        public int ImageWidth { get; set; } = 0;
        public int ImageHeight { get; set; } = 0;
        public int RedThreshold { get; set; } = -1;
        public int GreenThreshold { get; set; } = -1;
        public int BlueThreshold { get; set; } = -1;
        public int GreyThreshold { get; set; } = -1;
        public bool Help { get; set; } = false;
        public bool Inverse { get; set; } = false;
        public bool GreyScale { get; set; } = false;

    }
}
