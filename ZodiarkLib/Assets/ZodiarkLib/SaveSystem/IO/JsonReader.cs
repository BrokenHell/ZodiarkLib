using System;
using Newtonsoft.Json;
using ZodiarkLib.Core.IO;

namespace ZodiarkLib.SaveSystem.IO
{
    public class JsonReader : IReader
    {
        public T Read<T>(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return default;
            }

            var texts = System.IO.File.ReadAllText(path);
            return string.IsNullOrEmpty(texts) ? default : JsonConvert.DeserializeObject<T>(texts);
        }

        public object Read(string path, Type anonymousType)
        {
            if (!System.IO.File.Exists(path))
            {
                return default;
            }

            var texts = System.IO.File.ReadAllText(path);
            if (string.IsNullOrEmpty(texts))
                return null;
            
            var instance = Activator.CreateInstance(anonymousType);
            JsonConvert.PopulateObject(texts, instance);
            
            return instance;
        }
    }   
}