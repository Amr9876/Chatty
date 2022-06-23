
namespace Chatty.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Chatty.Services;

public partial class ChatScreen : ComponentBase, IAsyncDisposable
{

    private HubConnection hubConnection;
    private List<UserMessage> userMessages = new();
    private string messageInput;
    public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

    [Inject] public NavigationManager NavManager { get; set; }

    [Parameter] public string Name { get; set; }
    [Parameter] public string Email { get; set; }
    [Parameter] public string Id { get; set; }

    
    [CascadingParameter] public Task<AuthenticationState> authStateTask { get; set; }


    private AuthenticationState authState;

    [Inject] public UserService _userService { get; set; }

    private ClaimsPrincipal CurrentUser = new();

    protected override async Task OnInitializedAsync()
    {

        authState = await authStateTask;

        CurrentUser = authState.User;

        hubConnection = new HubConnectionBuilder().WithUrl(NavManager.ToAbsoluteUri("/chathub")).Build();
        
        hubConnection.On<string, string>("RecievePrivateMessage", async (string userId, string message) =>
        {
            if (authState is not null)
            {

                var user = await _userService.GetUserAsync(userId);

                userMessages.Add(new UserMessage
                {
                    UserName = user.FullName,
                    UserId = user.Id,
                    Message = message,
                    CurrentUser = CurrentUser.FindFirstValue(ClaimTypes.Name) == user.FullName
                });

            }


            await InvokeAsync(StateHasChanged);
        });

        await hubConnection.StartAsync();
    }

    private async Task Send()
    {
        if (!string.IsNullOrEmpty(messageInput) && CurrentUser is not null)
        {

            await hubConnection.SendAsync("SendPrivateMessage", CurrentUser.FindFirstValue(ClaimTypes.NameIdentifier), messageInput);

            messageInput = string.Empty;

        }
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
    
}
