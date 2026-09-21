using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public static class FileInitializer
    {
        public static async Task CopyFileIfNotExistsAsync(string fileName)
        {
            string targetPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(targetPath))
            {
                using Stream sourceStream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using FileStream destinationStream = File.Create(targetPath);
                await sourceStream.CopyToAsync(destinationStream);
            }
        }
    }
}
