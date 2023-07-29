using Web.ApiGateway.Extensions;
using Web.ApiGateway.Models.Catalog;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpClientFactory HttpClientFactory;
        private readonly IConfiguration Configuration;
        public CatalogService(IHttpClientFactory httpClientFactory,IConfiguration configuration)
        {
            this.HttpClientFactory = httpClientFactory;
            this.Configuration = configuration;
        }

      

        public async Task<CatalogItem> GetCatalogItemAsync(int id)
        {
            var client = HttpClientFactory.CreateClient("catalog");
            
            var response=await client.GetResponseAsync<CatalogItem>("/items/"+id);

            return response;
        }

        public Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(IEnumerable<int> id)
        {
            return null;
        }
    }
}
