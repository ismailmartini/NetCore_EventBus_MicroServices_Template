using System.Security.Claims;

namespace Web.ApiGateway.Infrastructure
{
    public class HttpClientDelegatingHandler:DelegatingHandler
    {

        private readonly IHttpContextAccessor _httpcontextAccessor;

        public HttpClientDelegatingHandler(IHttpContextAccessor contextAccessor)
        {
            _httpcontextAccessor = contextAccessor;
        }

        public HttpClientDelegatingHandler()
        {
            
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHandler= _httpcontextAccessor.HttpContext.Request.Headers["Authorization"];
            //if bearaer token s not null
            if(!string.IsNullOrEmpty(authorizationHandler))
            {
                if (request.Headers.Contains("Authorization"))
                {
                    request.Headers.Remove("Authorization");
                }
                request.Headers.Add("Authorization", new List<string>() { authorizationHandler });
            }

         



            return base.SendAsync(request, cancellationToken);
        }

    }
}
