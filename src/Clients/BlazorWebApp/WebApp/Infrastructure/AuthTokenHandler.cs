using Blazored.LocalStorage;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using WebApp.Extensions;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Infrastructure
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ISyncLocalStorageService storageService;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly ILocalStorageService localStorageService;
        public AuthTokenHandler(ISyncLocalStorageService identityService, ILocalStorageService localStorageService, IHttpContextAccessor contextAccessor)
        {
            this.storageService = identityService;
            this.localStorageService = localStorageService;

            _httpcontextAccessor = contextAccessor;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (localStorageService != null)
            {
                
                var token = localStorageService.GetToken(); 
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.ToString());
            }

           


            return base.SendAsync(request, cancellationToken);
        }
    }
}