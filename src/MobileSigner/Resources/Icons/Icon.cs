using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Resources.Icons
{
    public class Icon
    {

        private static Dictionary<string, string> fileMapping;
        
        static Icon()
        {
            fileMapping = new Dictionary<string, string>();
            fileMapping.Add("application/msword", "fileicon_doc.png");
            fileMapping.Add("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "fileicon_docx.png");
            fileMapping.Add("image/gif", "fileicon_gif.png");
            fileMapping.Add("image/jpeg", "fileicon_jpg.png");
            fileMapping.Add("video/mpeg", "fileicon_mpg.png");
            fileMapping.Add("application/vnd.oasis.opendocument.presentation", "fileicon_odp.png");
            fileMapping.Add("application/vnd.oasis.opendocument.spreadsheet", "fileicon_ods.png");
            fileMapping.Add("application/vnd.oasis.opendocument.text", "fileicon_odt.png");
            fileMapping.Add("application/pdf", "fileicon_pdf.png");
            fileMapping.Add("image/png", "fileicon_png.png");
            fileMapping.Add("text/plain", "fileicon_txt.png");
            fileMapping.Add("application/vnd.ms-excel", "fileicon_xls.png");
            fileMapping.Add("application/x-compressed", "fileicon_zip.png");
            fileMapping.Add("application/x-zip-compressed", "fileicon_zip.png");
            fileMapping.Add("application/zip", "fileicon_zip.png");
        }

        public static ImageSource FromMimeType(string mimeType)
        {
            string prefix = "Almg.MobileSigner.Resources.Icons.";
            if (fileMapping.ContainsKey(mimeType))
            {
                return ImageSource.FromResource(prefix + fileMapping[mimeType]);
            } else
            {
                return ImageSource.FromResource(prefix + "fileicon_any.png");
            }
        }
    }
}
