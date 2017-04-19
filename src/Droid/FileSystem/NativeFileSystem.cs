using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Almg.MobileSigner.Services;
using System.IO;
using Almg.MobileSigner.Droid.FileSystem;

[assembly: Xamarin.Forms.Dependency(typeof(NativeFileSystem))]
namespace Almg.MobileSigner.Droid.FileSystem
{

    public class NativeFileSystem : INativeFileSystem
    {
        public string GetCacheDir()
        {
			#if __ANDROID__
                return Android.App.Application.Context.CacheDir.AbsolutePath;
			#elif __IOS__
			    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var cacheDir = Path.Combine(documents, "..", "Library", "Caches");
			    return cacheDir; 
			#endif
        }

        public string GetFilesDir()
        {
            #if __ANDROID__
                var filesDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            #elif __IOS__
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filesDir = Path.Combine(documents, "..", "Library");
            #endif
            return filesDir;
        }

        public Stream GetFileStream(string path)
        {
            try
            {
                return File.Open(path, FileMode.OpenOrCreate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string JoinPaths(string path1, params string[] paths)
        {
            var path = path1;
            foreach(string path2 in paths) {
                path = Path.Combine(path, path2);
            }
            return path;
        }

        public void CreateFolder(string path)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void RemoveFile(string path)
        {
            File.Delete(path);
        }

        public void RemoveDirectory(string path)
        {
            Directory.Delete(path);
        }

        public int RemoveFilesOlderThan(string folderPath, DateTime date)
        {
            try
            {
                int count = 0;

                if (!DirectoryExists(folderPath))
                {
                    return 0;
                }

                string[] files = Directory.GetFileSystemEntries(folderPath);
                foreach (string file in files)
                {
                    if(IsDirectory(file))
                    {
                        DirectoryInfo di = new DirectoryInfo(file);
                        if(di.CreationTime < date)
                        {
                            count++;
                            di.Delete(true);
                        }
                    } else
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.CreationTime < date)
                        {
                            count++;
                            fi.Delete();
                        }
                    }
                }
                return count;
            } catch(Exception e)
            {
                throw e;
            }
        }

        public bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public long FileSize(string fileName)
        {
            return File.Open(fileName, FileMode.Open).Length;
        }
    }
}