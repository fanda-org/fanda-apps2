﻿using System.ComponentModel.DataAnnotations;

namespace FandaAuth.Service.ViewModels
{
    public class AuthenticateRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}