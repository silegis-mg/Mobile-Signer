using Almg.MobileSigner.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Controllers
{
    public class FileController
    {
        private INativeFileSystem fileSystem = DependencyService.Get<INativeFileSystem>();

        public void ClearOldCacheFiles()
        {
            var dateTime = DateTime.Now.AddDays(15);
            var dir = GetReqsDir();
            fileSystem.RemoveFilesOlderThan(dir, dateTime);
        }

        private string GetReqsDir()
        {
            string cacheDir = fileSystem.GetFilesDir();
            string reqPath = fileSystem.JoinPaths(cacheDir, "reqs");
            return reqPath;
        }
    }
}
