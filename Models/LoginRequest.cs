
using System.ComponentModel.DataAnnotations;

namespace Chatty.Models;

public class LoginRequest
{

    [EmailAddress, Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

}
