using System.ComponentModel.DataAnnotations;

namespace CafeteriaAromas.ViewModels;

public class LoginViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}