using System.Collections.Generic;
using Exify.Models;

namespace Exify.Contracts
{
    public interface IDirectoryService
    {
        /// <summary>
        /// Returns list of all supported extensions.
        /// </summary>
        /// <returns></returns>
        string[] SupportedExtensions { get; }
        /// <summary>
        /// Determines whether the given path is valid.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns></returns>
        bool IsPathValid(string path);
        /// <summary>
        /// Returns list of all direct child directories.
        /// </summary>
        /// <param name="directoryPath">The path to directory.</param>
        /// <returns></returns>
        List<string> GetSubDirectories(string directoryPath);
        /// <summary>
        /// Scans the given directory to find date range when the pictures inside this directory were taken.
        /// </summary>
        /// <param name="directoryPath">The path to directory.</param>
        /// <returns></returns>
        DirectoryScanResult ScanDirectory(string directoryPath);
        /// <summary>
        /// Based on the provided scan result, updates name of the directory by inserting start/end dates.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="scanResult"></param>
        void UpdateDirectoryName(string directoryPath, DirectoryScanResult scanResult);
    }
}
