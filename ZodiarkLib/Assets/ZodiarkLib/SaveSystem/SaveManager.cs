using System;
using System.IO;
using ZodiarkLib.Core.IO;
using UnityEngine;

namespace ZodiarkLib.SaveSystem
{
    public sealed class SaveManager : ISaveManager
    {
        #region Fields

        private IReader _reader;
        private IWriter _writer;
        private string _savePath;

        #endregion

        #region Public Methods

        
        public void Initialize(string saveFolder, IReader reader, IWriter writer)
        {
            _savePath = saveFolder;
            _reader = reader;
            _writer = writer;
        }

        public void Save(object data, string appendPath = "")
        {
            if (_writer == null) return;
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, data.GetType().Name + ".dat");
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            _writer.Write(data, path);
        }

        public T Load<T>( string appendPath = "") where T : new()
        {
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, typeof(T).Name + ".dat");
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            return _reader.Read<T>(path);
        }

        public object Load(Type type, string appendPath = "")
        {
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, type.Name + ".dat");
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            return _reader.Read(path, type);
        }

        #endregion
    }   
}