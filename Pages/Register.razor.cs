
namespace Chatty.Pages;

using Microsoft.AspNetCore.Components;
using Models;
using Services;

public partial class Register
{

    private RegisterRequest request = new();

    [Inject] public AuthService authService { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }

    async Task OnSubmit()
    {

        await authService.CreateUserAsync(request);

        NavManager.NavigateTo("/login");

    }

}