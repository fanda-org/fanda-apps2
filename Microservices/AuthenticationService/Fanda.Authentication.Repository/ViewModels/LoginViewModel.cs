using System.ComponentModel.DataAnnotations;

namespace Fanda.Authentication.Repository.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        //[EmailAddress]
        [Display(Name = "Name / Email")]
        public string NameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
