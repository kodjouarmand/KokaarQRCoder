using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace KokaarQrCoder.Utility.Helpers
{
    public static class QRCodeHelper
    {
        public static void GenerateQRCode(string payload, string path = null, bool save = false)
        {
            QRCodeGenerator qrCodeGenerator = new();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload.ToString(), QRCodeGenerator.ECCLevel.M, true);
            QRCode qrCode = new(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(5);

            if (save && path != null)
                qrCodeImage.Save(path, ImageFormat.Png);
        }

        public static void GetPayload<T>(List<T> list,string label, string value, ref StringBuilder payload)
        {
            payload.Append('\n');
            foreach (var item in list)
            {
                PropertyInfo labelProperty = item.GetType().GetProperties().Where(p => p.Name.Equals(label)).FirstOrDefault();
                PropertyInfo valueProperty = item.GetType().GetProperties().Where(p => p.Name.Equals(value)).FirstOrDefault();                

                payload.Append($"{labelProperty.GetValue(item, null)} : {valueProperty.GetValue(item, null)}\n");
            }
        }
    }
}
