using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Helpers
{
    public class FileHelper
    {
        public static async Task<string> Save(string folderName, string fileName, Stream stream)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                StreamHelper.CopyStream(stream, fileStream);
                fileStream.Flush();
            };
            return file.Path;
        }

        public static string SanitizeFileName(string filename)
        {
            return String.Join("_", filename.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}
