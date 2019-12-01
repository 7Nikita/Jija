using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Account
{
    public class LoginForm
    {
        [Required] public string UserName { get; set; }
        
        [Required] public string Password { get; set; }
    }
}