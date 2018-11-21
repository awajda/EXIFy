using System;
using Exify.Contracts;
using Exify.Services;

namespace Exify
{
    class Program
    {
        private static IDirectoryService _dirService = new DirectoryService();
        static void Main(string[] args)
        {
            Console.WriteLine("---- EXIFy v 1.0 ----");
            Console.WriteLine("Please provide path to directory that you want to process.");
            Console.WriteLine("App will scan the given directory in order to rename all direct child directories.");
            
            var pathToProcess = _getPathToExify();
            if (!_dirService.IsPathValid(pathToProcess))
            {
                Console.WriteLine("The path that does not exist. Stopping...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Scanning directory to find direct child directories");
            Console.WriteLine(
                $"Supported file extension(s) to read EXIF data: {string.Join(",", _dirService.SupportedExtensions)}");

            var subDirectoriesToProcess = _dirService.GetSubDirectories(pathToProcess);
            subDirectoriesToProcess.ForEach(x => Console.WriteLine($"Found directory '{x}' and adding it to the processing queue"));

            foreach (var subDirectory in subDirectoriesToProcess)
            {
                Console.WriteLine($"Processing directory: {subDirectory}");
                var scanResult = _dirService.ScanDirectory(subDirectory);
                if (scanResult != null)
                {
                    _dirService.UpdateDirectoryName(subDirectory, scanResult);
                }
            }

            // let the app stay opened until user presses any key
            Console.ReadLine();
        }

        /// <summary>
        /// Ask user to provide a path to directory that will be processed by the application.
        /// </summary>
        /// <returns>A path to directory</returns>
        private static string _getPathToExify()
        {
            string path;
            do
            {
                path = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(path))
                    Console.WriteLine("Path can't be empty. Please enter a valid path:");

            } while (string.IsNullOrWhiteSpace(path));

            return path;
        }
    }
}
