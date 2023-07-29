using System.Threading.Tasks;
using WebApp.Domain.Models;
using WebApp.Domain.Models.CatalogModels;

namespace WebApp.Application.Services.interfaces
{
    public interface ICatalogService
    {
        Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItems();
    }
}
