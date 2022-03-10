using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace KokaarQrCoder.Utility.Helpers
{
    public class FileHelper
    {
        public static List<T> GetJsonData<T>(string fullFileName)
        {
            string data = File.ReadAllText(fullFileName, Encoding.GetEncoding("iso-8859-1"));
            List<T> list = JsonSerializer.Deserialize<List<T>>(data);
            return list;
        }
    }
}
