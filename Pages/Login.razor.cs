
namespace Chatty.Pages;

using Microsoft.AspNetCore.Components;
using Services;
using Models;

public partial class Login
{

    private LoginRequest request = new();

    [Inject] public AuthService authService { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }
    
    async Task OnSubmit()
    {

        await authService.AuthenticateAsync(request);

        NavManager.NavigateTo("/", true);

    }

}
