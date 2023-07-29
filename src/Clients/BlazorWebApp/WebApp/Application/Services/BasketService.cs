using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Application.Services.Dtos;
using WebApp.Application.Services.interfaces;
using WebApp.Domain.Models.ViewModels;
using WebApp.Extensions;

namespace WebApp.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient apiClient;
        private readonly IIdentityService identityService;
        private readonly ILogger<BasketService> logger;

        public BasketService(HttpClient apiClient, IIdentityService identityService, ILogger<BasketService> logger)
        {
            this.apiClient = apiClient;
            this.identityService = identityService;
            this.logger = logger;
        }

        
        public async Task AddItemToBasket(int productId)
        {
            
            //htttp aggragete method (api gateway project=>controlers)
            var model = new
            {
                CatalogItemId = productId,
                Quatity = 1,
                BasketId = identityService.GetUserName()
            };
          
             await apiClient.PostAsync("basket/items", model);
              
        }

        public Task CheckOut(BasketDTO basket)
        {
            return apiClient.PostAsync("basket/checkout", basket);
        }

        public async Task<Basket> GetBasket()
        {
            ////apiClient.DefaultRequestHeaders.Add("Authorization", identityService.GetUserToken());
            apiClient.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", identityService.GetUserToken());
            var response =await apiClient.GetResponseAsync<Basket>("basket/" + identityService.GetUserName());

            return response ?? new Basket() { BuyerId = identityService.GetUserName() };
        }

        public async Task<Basket> UpdateBasket(Basket basket)
        {
            var response = await apiClient.PostGetResponseAsync<Basket,Basket>("basket/update" ,basket);
            return response;
        }
    }
}
