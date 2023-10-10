using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;


namespace Foodshare.Handlers
{
   
    public class JwtTokenHandler
    {
        // private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
        

        public JwtTokenHandler(CustomAuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public string GetToken()
        {
            var authState = _authenticationStateProvider.GetAuthenticationStateAsync().Result;
            var user = authState.User;

            var token = user.FindFirst("token")?.Value;
            return token;
        }

        public JwtSecurityToken GetDecodedToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = GetToken();

            var decodedToken = tokenHandler.ReadJwtToken(token);
            return decodedToken;
        }

        public bool IsTokenValid()
        {
            var token = GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                var decodedToken = GetDecodedToken();
                return decodedToken.ValidTo > DateTime.UtcNow;
            }

            return false;
        }
    }
}