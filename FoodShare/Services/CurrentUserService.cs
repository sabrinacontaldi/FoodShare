using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Services
{
    public class CurrentUserService
    {
        public static UserInfo _currentUser {get; set;}

        public static UserInfo2 _cUser {get; set;}
        // private string _accessToken;

        public UserInfo CurrentUser => _currentUser;

        public UserInfo2 CUser => _cUser;

        // public string AccessToken => _accessToken;

        // public void SetCurrentUser(string accessToken)
        // {
        //     _currentUser = DecodeAccessToken(accessToken);
        //     _accessToken = accessToken;
        //     //using access token:
        //     // Get role, id, username
        // }

        public void SetCurrentUser(UserInfo cUser)
        {
            _currentUser = cUser;
        }
        public void SetCUser(UserInfo2 cUser)
        {
            _cUser = cUser;
        }


        public void ClearCurrentUser()
        {
            _currentUser = null;
            // _accessToken = null;
        }
        public void ClearCUser()
        {
            _cUser = null;
            // _accessToken = null;
        }

        // public UserInfo DecodeAccessToken(string jwtToken)
        // {
        //     JwtPayload payload = DecodeJwtToken(jwtToken);

        //     string username = payload["username"]?.ToString();
        //     string role = payload["role"]?.ToString();
        //     string userId = payload["id"]?.ToString();

        //     return new UserInfo(userId, username, role);
        // }

        // public static JwtPayload DecodeJwtToken(string jwtToken)
        // {
        //     JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //     JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

        //     return jwtSecurityToken.Payload;
        // }
    }

    public class UserInfo
    {
        //feeder/donor id
        public int Id { get; set; }
        //usernam
        public string Username {get; set;}
        //authentication role
        public string Role { get; set; }

        //zip code for filtering
        // private int zipCode { get; set; }
        public UserInfo(){}
        public UserInfo(int id, string username, string role)
        {
            Id = id;
            Username = username;
            Role = role;
        }
    }

     public class UserInfo2
    {
        //feeder/donor id
        public int Id { get; set; }
        //usernam
        // public int zipCode {get; set;}
        //authentication role
        // public string Role { get; set; }

        //zip code for filtering
        // private int zipCode { get; set; }
        public UserInfo2(){}
        public UserInfo2(int id)
        {
            Id = id;
            // zipCode = zip;

        }
    }
}