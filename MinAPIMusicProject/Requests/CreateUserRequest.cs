namespace MinAPIMusicProject.Requests;
using System.ComponentModel.DataAnnotations;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Nickname is required.")]
    [MinLength(4, ErrorMessage = "Nickname must be at least 4 characters long.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
    public string Password { get; set; }
}