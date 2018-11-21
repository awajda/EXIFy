using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Exify.Contracts;
using Exify.Helpers;
using Exify.Models;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Directory = System.IO.Directory;

namespace Exify.Services
{
    public class DirectoryService : IDirectoryService
    {
        public string[] SupportedExtensions => new[] {"jpg", "jpeg"};
        public const string ExifDateTimeTagName = "Date/Time Original";
        protected IValueParser<string, DateTime?> ExifDateTimeParser;

        public DirectoryService()
        {
            ExifDateTimeParser = new ExifDateTimeParser();
        }

        public bool IsPathValid(string path)
        {
            return Directory.Exists(path);
        }

        public List<string> GetSubDirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly).ToList();
        }

        private List<string> _getFilesToProcess(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).Where(filename =>
                SupportedExtensions.Any(supportedExtension =>
                    filename.EndsWith(supportedExtension, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        public DirectoryScanResult ScanDirectory(string directoryPath)
        {
            var filesToScan = _getFilesToProcess(directoryPath);
            Console.WriteLine($"Found {filesToScan.Count} files to process.");

            var result = new DirectoryScanResult();

            foreach (var file in filesToScan)
            {
                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(file);
                    var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    var dateTakenText = subIfdDirectory?.Tags?.FirstOrDefault(x =>
                        string.Equals(x.Name, ExifDateTimeTagName, StringComparison.OrdinalIgnoreCase))?.Description;

                    DateTime? parsedDateTime = ExifDateTimeParser.Parse(dateTakenText);
                    if(parsedDateTime.HasValue)
                    {
                        if (!result.StartingDate.HasValue || result.StartingDate > parsedDateTime.Value)
                            result.StartingDate = parsedDateTime.Value;

                        if (!result.EndingDate.HasValue || result.EndingDate < parsedDateTime.Value)
                            result.EndingDate = parsedDateTime.Value;
                    }
                }
                catch (ImageProcessingException ex)
                {
                    Console.WriteLine($"Error while processing the file '{file}': {ex.Message}");
                }
            }
            return result;
        }

        public void UpdateDirectoryName(string directoryPath, DirectoryScanResult scanResult)
        {
            var directoryName = new DirectoryInfo(directoryPath).Name;
            if (scanResult.StartingDate.HasValue && scanResult.EndingDate.HasValue)
            {
                // start and end dates are set
                if (scanResult.StartingDate.Value.Year != scanResult.EndingDate.Value.Year)
                {
                    Console.WriteLine("Detected a directory that has pictures taken in different years, skipping.");
                    return;
                }

                string suggestedDirectoryName;
                if (scanResult.StartingDate.Value.Month != scanResult.EndingDate.Value.Month)
                {
                    suggestedDirectoryName =
                        $"{scanResult.StartingDate.Value.ToString("yyyy-MM-dd")}-{scanResult.EndingDate.Value.ToString("yyyy-MM-dd")} {directoryName}";
                }
                else if (scanResult.StartingDate.Value.Day != scanResult.EndingDate.Value.Day)
                {
                    suggestedDirectoryName =
                        $"{scanResult.StartingDate.Value.ToString("yyyy-MM-dd")}-{scanResult.EndingDate.Value.ToString("yyyy-MM-dd")} {directoryName}";
                }
                else
                {
                    suggestedDirectoryName = $"{scanResult.StartingDate.Value.ToString("yyyy-MM-dd")} {directoryName}";
                }

                Console.WriteLine($"Changing {directoryName} -> {suggestedDirectoryName}");
                Directory.Move(directoryPath, directoryPath.Replace(directoryName, suggestedDirectoryName));
            }
            else
            {
                Console.WriteLine($"Directory {directoryName} did not return any scan result, no need to change its name.");
            }
        }
    }
}
