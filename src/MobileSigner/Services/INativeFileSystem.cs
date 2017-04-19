using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Services
{
    public interface INativeFileSystem
    {
        /// <summary>
        /// Gets the plataform specific cache dir
        /// </summary>
        /// <returns></returns>
        string GetCacheDir();

        /// <summary>
        /// Gets the platform specific general file storage folder
        /// </summary>
        /// <returns></returns>
        string GetFilesDir();

        /// <summary>
        /// Opens a stream to a file at the provided path.
        /// If write is true and the file does not exists, it will be created.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Stream</returns>
        Stream GetFileStream(string path);

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool FileExists(string path);
              
        /// <summary>
        /// Check if directory exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Remove all files older than the provided date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        int RemoveFilesOlderThan(string folderPath, DateTime date);

        /// <summary>
        /// Removes file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>If the file was deleted</returns>
        void RemoveFile(string path);

        /// <summary>
        /// Removes directory
        /// </summary>
        /// <param name="path">Directory path</param>
        void RemoveDirectory(string path);

        /// <summary>
        /// Join two paths according to the platform
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        string JoinPaths(string path1, params string[] paths);

        /// <summary>
        /// Creates recursively a new folder
        /// </summary>
        /// <param name="path"></param>
        void CreateFolder(string path);

        long FileSize(string fileName);
    }
}
