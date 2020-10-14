using System.ComponentModel.DataAnnotations;

namespace Fanda.Authentication.Repository.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
