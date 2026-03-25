using System;
using System.Collections.Generic;
using System.Text;

namespace OrderonTailoringCostCalculation.Services
{
    public static class DataBaseInitializer
    {
        public static async Task CopyDatabaseIfNotExistsAsync(string databaseFileName)
        {
            string targetPath = Path.Combine(FileSystem.AppDataDirectory, databaseFileName);

            if (!File.Exists(targetPath))
            {
                using Stream sourceStream = await FileSystem.OpenAppPackageFileAsync(databaseFileName);
                using FileStream destinationStream = File.Create(targetPath);
                await sourceStream.CopyToAsync(destinationStream);
            }
        }
    }
}
