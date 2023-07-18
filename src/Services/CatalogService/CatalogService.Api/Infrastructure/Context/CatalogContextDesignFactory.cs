using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer("Data Source=DESKTOP-8S3OCJ3\\SQLEXPRESS;Initial Catalog=catalog;Persist Security Info=True;User ID=sa;Password=1");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
