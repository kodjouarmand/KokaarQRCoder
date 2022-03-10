
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace KokaarQrCoder.Utility.Options
{
    public class LoggingOptions
    {
        public const string ConfigSectionName = "LoggingOptions";
        public static string DefaultLogFileName => $"{DateTime.Now.ToShortDateString().Replace("/","-")}_kokaar_qr_coder_log.txt";
        public string LogFileName { get; set; }
        public static string GetLogPath(IWebHostEnvironment hostEnvironment, string filename)
        {
            string webRootPath = hostEnvironment.WebRootPath;
            var path = Path.Combine(webRootPath, $"logs\\{filename}");
            return path;
        }
    }
}
