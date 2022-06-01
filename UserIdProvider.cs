namespace Chatty;

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {

        string userId = connection.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId;

    }
}
