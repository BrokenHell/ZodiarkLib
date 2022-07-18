using Newtonsoft.Json;
using ZodiarkLib.Core.IO;

namespace ZodiarkLib.SaveSystem.IO
{
    public class JsonWriter : IWriter
    {
        public void Write(object data, string path)
        {
            if (!System.IO.File.Exists(path))
            {
                var dir = path.Remove(path.LastIndexOf('/'));
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }

            var json = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(path, json);
        }
    }   
}
