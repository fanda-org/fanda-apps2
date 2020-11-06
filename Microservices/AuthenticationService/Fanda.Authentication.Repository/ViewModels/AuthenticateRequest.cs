using System.ComponentModel.DataAnnotations;

namespace Fanda.Authentication.Repository.ViewModels
{
    public class AuthenticateRequest
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}