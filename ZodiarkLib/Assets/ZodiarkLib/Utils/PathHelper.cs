using System.IO;
using UnityEngine;

namespace ZodiarkLib.Utils
{
    public static class PathHelper
    {
        /// <summary>
        /// Return a path to a writable folder on a supported platform
        /// </summary>
        /// <param name="relativeFilePath">A relative path to the file, from the out most writable folder</param>
        /// <returns></returns>
        public static string GetWritablePath(string relativeFilePath)
        {
            string result = "";
#if UNITY_EDITOR
            result = Application.dataPath.Replace("Assets", "PersistentData") + "/" + relativeFilePath;
#elif UNITY_ANDROID
		    result = Application.persistentDataPath + "/" + relativeFilePath;
#elif UNITY_IOS
		    result = Application.persistentDataPath + "/" + relativeFilePath;
#elif UNITY_WP8 || NETFX_CORE
		    result = Application.persistentDataPath + "/" + relativeFilePath;
#else
		    result = Application.persistentDataPath + "/" + relativeFilePath;
#endif
            return result;
        }

        /// <summary>
        /// Return a path to a writable folder on a supported platform
        /// </summary>
        /// <returns></returns>
        public static string GetWritableFolder()
        {
            string result = "";
#if UNITY_EDITOR
            result = Application.dataPath.Replace("Assets", "PersistentData");
#elif UNITY_ANDROID
		    result = Application.persistentDataPath ;
#elif UNITY_IOS
		    result = Application.persistentDataPath ;
#elif UNITY_WP8 || NETFX_CORE
		    result = Application.persistentDataPath ;
#else
		    result = Application.persistentDataPath ;
#endif
            return result;
        }

        public static string GetStreamingFolder()
        {
            return Application.streamingAssetsPath;
        }

        /// <summary>
        /// Read a file at specified path
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="isAbsolutePath">Is this path an absolute one?</param>
        /// <returns>Data of the file, in byte[] format</returns>
        public static byte[] LoadFile(string filePath, bool isAbsolutePath = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            var absolutePath = filePath;
            if (!isAbsolutePath) { absolutePath = GetWritablePath(filePath); }

            return File.Exists(absolutePath) ? File.ReadAllBytes(absolutePath) : null;
        }
        
        public static string[] LoadFileStrings(string filePath, bool isAbsolutePath = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            var absolutePath = filePath;
            if (!isAbsolutePath) { absolutePath = GetWritablePath(filePath); }

            return File.Exists(absolutePath) ? File.ReadAllLines(absolutePath) : null;
        }

        /// <summary>
        /// Get all files at directory
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="isAbsolutePath"></param>
        /// <returns></returns>
        public static string[] GetFiles(string directoryPath, bool isAbsolutePath = false)
        {
            if (string.IsNullOrEmpty(directoryPath)) return null;
            var absolutePath = directoryPath;
            if (!isAbsolutePath)
            {
                absolutePath = GetWritablePath(directoryPath);
            }

            return Directory.Exists(absolutePath) ? Directory.GetFiles(absolutePath) : null;
        }

        /// <summary>
        /// Check if file is exist
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isAbsolutePath"></param>
        /// <returns></returns>
        public static bool IsFileExist(string filePath, bool isAbsolutePath = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            var absolutePath = filePath;
            if (!isAbsolutePath) { absolutePath = GetWritablePath(filePath); }

            var folderName = Path.GetDirectoryName(absolutePath);

            if (!string.IsNullOrEmpty(folderName) && !Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Save a byte array to storage at specified path and return the absolute path of the saved file
        /// </summary>
        /// <param name="bytes">Data to write</param>
        /// <param name="filePath">Where to save file</param>
        /// <param name="isAbsolutePath">Is this path an absolute one or relative</param>
        /// <returns>Absolute path of the file</returns>
        public static string SaveFile(byte[] bytes, string filePath, bool isAbsolutePath = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            var path = filePath;
            if (!isAbsolutePath)
            {
                path = GetWritablePath(filePath);
            }

            //create a directory tree if not existed
            var folderName = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(folderName) && !Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            //write file to storage
            File.WriteAllBytes(path, bytes);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
            return path;
        }


        /// <summary>
        /// Delete a file from storage using default setting
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <param name="isAbsolutePath">Is this file path an absolute path or relative one?</param>
        public static bool DeleteFile(string filePath, bool isAbsolutePath = false)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(filePath)) return false;

                var file = GetWritablePath(filePath);
                if (isAbsolutePath)
                {
                    if (!File.Exists(filePath)) return false;

                    File.Delete(filePath);
                    return true;
                }
                else
                {
                    filePath = file;
                    isAbsolutePath = true;
                }
            }
        }
    }
}