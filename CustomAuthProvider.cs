
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Chatty;

public class CustomAuthProvider : AuthenticationStateProvider
{

    private readonly ProtectedLocalStorage _localStorage;

    public CustomAuthProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        var state = new AuthenticationState(new());
        var token = await _localStorage.GetAsync<string>("access_token");

        if(!string.IsNullOrEmpty(token.Value))
        {

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token.Value);

            var identity = new ClaimsIdentity(jwtToken.Claims, JwtBearerDefaults.AuthenticationScheme);

            state = new AuthenticationState(new(identity));

        }

        return state;

    }

}
