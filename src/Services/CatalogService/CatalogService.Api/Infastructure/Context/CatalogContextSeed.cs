using CatalogService.Api.Core.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Globalization;
using System.IO.Compression;

namespace CatalogService.Api.Infastructure.Context
{
    public class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogContext context, IWebHostEnvironment env, ILogger<CatalogContextSeed> logger)
        {
            var policy = Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retry => System.TimeSpan.FromSeconds(5),
                onRetry: (Exception, timespan, retry, ctx) =>
                {

                });

            var setupDirPath = Path.Combine(env.ContentRootPath, "Infastructure", "Setup");
            var picturePath = "Pics";

            await policy.ExecuteAsync(() => ProcessSeeding(context, setupDirPath, picturePath, logger));

        }

        private async Task ProcessSeeding(CatalogContext context, string setupDirPath, string picturePath, ILogger logger)
        {
            if (!context.CatalogBrands.Any())
            {
                await context.CatalogBrands.AddRangeAsync(GetCatalogBrandsFromFile(setupDirPath));
            await context.SaveChangesAsync();

            }
            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetCatalogTypesFromFile(setupDirPath));
            await context.SaveChangesAsync();

            }
            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(GetCatalogItemsFromFile(setupDirPath,context));
            await context.SaveChangesAsync();

            }
        }

        private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string setupDirPath)
        {
            string fileName = Path.Combine(setupDirPath, "BrandsTextFile.txt");
            var fileContent = File.ReadAllLines(fileName);
            var list = fileContent.Select(i => new CatalogBrand()
            {
                Brand = i.Trim('"')
            }).Where(i => i != null);
            return list;
        }

        private IEnumerable<CatalogType> GetCatalogTypesFromFile(string setupDirPath)
        {
            string fileName = Path.Combine(setupDirPath, "CatalogTypes.txt");
            var fileContent=File.ReadAllLines(fileName);
            var list = fileContent.Select(i => new CatalogType()
            {
                Type = i.Trim('"')
            }).Where(i => i != null);
            return list;
        }

        private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string setupDirPath, CatalogContext catalogContext)
        {
          
            return null;
        }

        private void GetCatalogItemPictures(string contentPath, string picturePath)
        {
            picturePath ??= "pics";

            if (picturePath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(picturePath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                string zipFileCatalogItemPictures = Path.Combine(picturePath, "CatalogItems,zip");
                ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
            }
        }
    }
}
