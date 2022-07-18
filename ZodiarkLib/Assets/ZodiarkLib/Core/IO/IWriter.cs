namespace ZodiarkLib.Core.IO
{
    /// <summary>
    /// Base Reader , we can implement this class for I/O using in-game
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Write the specified data in given path.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="path">Path.</param>
        void Write(object data , string path);
    }
}