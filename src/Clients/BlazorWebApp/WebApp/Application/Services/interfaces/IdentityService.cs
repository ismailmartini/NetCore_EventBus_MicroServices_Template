using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Domain.Models.User;
using WebApp.Extensions;
using WebApp.Utils;

namespace WebApp.Application.Services.interfaces
{
    public class IdentityService : IIdentityService
    {

        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService syncLocalStorageService; 
        private readonly AuthenticationStateProvider authStateProvider;

        public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.syncLocalStorageService = syncLocalStorageService;
            this.authStateProvider = authStateProvider;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());    


        public string GetUserName()
        {
           return syncLocalStorageService.GetUsername();
        }

        public string GetUserToken()
        {
            return syncLocalStorageService.GetToken();
        }

        public async Task<bool> Login(string username, string password)
        {
            var req = new UserLoginRequest(username, password);
            var response = await httpClient.PostGetResponseAsync<UserLoginResponse, UserLoginRequest>("auth", req);
            if(!string.IsNullOrEmpty(response.UserName))
            {
                syncLocalStorageService.SetToken(response.Token);
                syncLocalStorageService.SetUsername(response.UserName);
                ((AuthStateProvider)authStateProvider).NotifyUserLogin(response.UserName);

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.Token);
                return true;
            }
            return false;
        }

        public void Logout()
        {
            syncLocalStorageService.RemoveItem("token");
            syncLocalStorageService.RemoveItem("username");

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;

        }
    }
}
