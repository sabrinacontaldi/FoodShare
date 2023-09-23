using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace FoodShare.Handlers
{
    public class CustomAuthorizationHandler : DelegatingHandler
    {
        public ILocalStorageService _localStorageService;

        public CustomAuthorizationHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //getting token from the localstorage
            var jwtToken = await _localStorageService.GetItemAsync<string>("jwt_token");

            //adding the token in authorization header
            if(jwtToken != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            //sending the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}