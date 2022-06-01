using System.Security.Claims;
using Chatty.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chatty.Components;

public partial class ChatMenu : ComponentBase
{

    private List<User> users;

    [CascadingParameter]
    public Task<AuthenticationState> AuthStateTask { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateTask;

        if(authState.User is not null && authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {

            users = (await userService.GetUsersAsync()).Where(x => x.EmailAddress != authState.User.FindFirstValue(ClaimTypes.Email)).ToList();

        } else 
        {

            users = await userService.GetUsersAsync();

        }

    }

    public void NavigateToChatRoom(string id)
    {

        NavManager.NavigateTo($"/chat/{id}", true);       

    }   

}