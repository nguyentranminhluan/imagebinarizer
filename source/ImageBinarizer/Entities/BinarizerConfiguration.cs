using ImageBinarizerLib.Entities;

namespace ImageBinarizerApp.Entities
{
    /// <summary>
    /// Collect requested Binarization data
    /// </summary>
    public class BinarizerConfiguration : BinarizerParams
    {
        /// <summary>
        /// Enable to print help function.
        /// </summary>
        public bool Help { get; set; } = false;

    }
}
