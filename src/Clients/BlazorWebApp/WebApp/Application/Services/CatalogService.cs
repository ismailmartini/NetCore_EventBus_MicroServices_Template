using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Application.Services.interfaces;
using WebApp.Domain.Models;
using WebApp.Domain.Models.CatalogModels;
using WebApp.Extensions;

namespace WebApp.Application.Services
{
    public class CatalogService : ICatalogService
    {
        public HttpClient ApiClient { get; }

        public CatalogService(HttpClient apiClient)
        {
            this.ApiClient = apiClient;
        }


        public async Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItems()
        {
           var response =await ApiClient.GetResponseAsync<PaginatedItemsViewModel<CatalogItem>>("/catalog/items");
            return response;
        }
    }
}
