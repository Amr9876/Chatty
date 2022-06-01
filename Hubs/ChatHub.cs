
namespace Chatty.Hubs;

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class ChatHub : Hub
{

    public async Task SendPrivateMessage(string userId, string message)
    {

        await Clients.Users(userId, Context.User.FindFirstValue(ClaimTypes.NameIdentifier))
                     .SendAsync("RecievePrivateMessage", userId, message);

    }

}
