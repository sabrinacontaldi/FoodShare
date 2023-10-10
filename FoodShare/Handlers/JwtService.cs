using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace FoodShare.Handlers
{
    public class JwtService
    {
        private readonly IJSRuntime _jsRuntime;

        public JwtService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetJwtAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
        }

    }
}