using ImageBinarizerLib.Entities;

namespace ImageBinarizerApp.Entities
{
    /// <summary>
    /// Collect requested Binarize data
    /// </summary>
    public class BinarizerConfiguration : BinarizerParams
    {
        #region Public properties
        /// <summary>
        /// Enable to print help function
        /// </summary>
        public bool Help { get; set; } = false;
        #endregion

    }
}
