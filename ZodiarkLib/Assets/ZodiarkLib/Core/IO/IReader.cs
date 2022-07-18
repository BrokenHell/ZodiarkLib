using System;

namespace ZodiarkLib.Core.IO
{
    /// <summary>
    /// Base Reader , we can implement this class for I/O using in-game
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Read and deserialize object type T from <paramref name="path"/>
        /// </summary>
        /// <returns>The deserialize object</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">Type of an object</typeparam>
        T Read<T>(string path);

        /// <summary>
        /// Read and deserialize anonymous type object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="anonymousType"></param>
        object Read(string path, Type anonymousType);
    }
}