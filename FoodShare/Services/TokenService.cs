// 3rd token service

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace FoodShare.Services
{
        public interface ITokenService
    {
        Task<TokenDTO> GetToken();
        Task RemoveToken();
        Task SetToken(TokenDTO tokenDTO);
    }
 
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService localStorageService;
 
        public TokenService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }
 
        public async Task SetToken(TokenDTO tokenDTO)
        {
            await localStorageService.SetItemAsync("token", tokenDTO);
        }
 
        public async Task<TokenDTO> GetToken()
        {
            return await localStorageService.GetItemAsync<TokenDTO>("token");
        }
 
        public async Task RemoveToken()
        {
            await localStorageService.RemoveItemAsync("token");
        }
    }

    public class TokenDTO
    {
        public string Token { get; set; }
 
        public DateTime Expiration { get; set; }
    }
    
}