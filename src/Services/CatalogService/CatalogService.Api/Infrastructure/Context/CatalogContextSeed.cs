using CatalogService.Api.Core.Domain;
using Microsoft.Data.SqlClient;
using Polly;
using System.Globalization;
using System.IO.Compression;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextSeed
    {

        public async Task SeedAsync(CatalogContext context, IHostEnvironment env, ILogger<CatalogContextSeed> logger)
        {

            var policy = Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "CatalgoContext  insert Exception  with message insert detected ");
                    }
                    );

            var setupDirPath = Path.Combine(env.ContentRootPath, "Infrastructure", "Setup", "SeedFiles");
            var picturePath = "Pics";
            await policy.ExecuteAsync(() => ProcessSeeding(context, setupDirPath, picturePath, logger));
        }



        private async Task ProcessSeeding(CatalogContext context,string setupDirPath,string picturePath,ILogger logger)
        {
            if(!context.CatalogBrands.Any()) {

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

                await context.CatalogItems.AddRangeAsync(GetCatalogItemFromFile(setupDirPath,context));
                await context.SaveChangesAsync();

                GetCatalogItemPictures(setupDirPath, picturePath);
            }


        }


        private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentPath)
        {


            IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
            {
                return new List<CatalogBrand>()
            {
                new CatalogBrand {Brand="Azure"},
                new CatalogBrand {Brand=".NET"  }

            };
            }



            string fileName = Path.Combine(contentPath, "BrandsTextFile.txt");
            if (!File.Exists(fileName))
            {
                return GetPreconfiguredCatalogBrands();
            }

            var fileContent = File.ReadAllLines(fileName);
            var list = fileContent.Select(i => new CatalogBrand()
            {
                Brand = i.Trim('"')
            }).Where(i => i != null);

            return list ?? GetPreconfiguredCatalogBrands();
        }



        private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentPath)
        {


            IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
            {
                return new List<CatalogType>()
            {
                new CatalogType {Type="Mug"},
                new CatalogType {Type="T-Shirt"  }

            };
            }



            string fileName = Path.Combine(contentPath, "CatalogTypes.txt");
            if(!File.Exists(fileName))
            {
                return GetPreconfiguredCatalogTypes();
            }

            var fileContent = File.ReadAllLines(fileName);
            var list = fileContent.Select(i => new CatalogType()
            {
                Type = i.Trim('"')
            }).Where(i => i != null);

            return list ?? GetPreconfiguredCatalogTypes();
        }


        private IEnumerable<CatalogItem> GetCatalogItemFromFile(string contentPath,CatalogContext context)
        {


            IEnumerable<CatalogItem> GetPreconfiguredItems()
            {
                return new List<CatalogItem>()
            {
                new CatalogItem {CatalogTypeId=2,CatalogBrandId=2 ,AvailableStock=100,Description=".NET bot Black Hoodie",Name=".NET Hoodie",Price=19,PictureFileName="1.png"},
                new CatalogItem {CatalogTypeId=1,CatalogBrandId=2 ,AvailableStock=100,Description=".NET Black Hoodie",Name=".NET Black Hoodie",Price=2,PictureFileName="2.png"}

            };
            }

            string fileName = Path.Combine(contentPath, "CatalogItems.txt");
            if(!File.Exists(fileName))
            {
                return GetPreconfiguredItems();
            }

            var catalogTypeIdLookup = context.CatalogTypes.ToDictionary(ct => ct.Type , ct => ct.Id);
            var catalogBrandIdLookup = context.CatalogBrands.ToDictionary(ct => ct.Brand , ct => ct.Id);

            var fileContent = File.ReadAllLines(fileName)
                .Skip(1) //skip header
                .Select(i => i.Split(','))
                .Select(i => new CatalogItem()
                {
                    CatalogTypeId = catalogTypeIdLookup[i[0]],
                    CatalogBrandId = catalogTypeIdLookup[i[1]],
                    Description = i[2].Trim('"').Trim(),
                    Name = i[3].Trim('"').Trim(),
                    Price = Decimal.Parse(i[4].Trim('"').Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                    PictureFileName = i[5].Trim('"').Trim(),
                    AvailableStock = string.IsNullOrEmpty(i[6]) ? 0 : int.Parse(i[6]),
                    OnReoder = Convert.ToBoolean(i[7])
                });

            return fileContent;
        }



        private void GetCatalogItemPictures(string contentPath,string picturePath)
        {
            picturePath ??= "pics";

            if (picturePath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(picturePath);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                string zipFileCatalogItemPictures = Path.Combine(contentPath, "CatalogItems.zip");
                ZipFile.ExtractToDirectory(zipFileCatalogItemPictures,picturePath); 
            }
        }
    }
}
