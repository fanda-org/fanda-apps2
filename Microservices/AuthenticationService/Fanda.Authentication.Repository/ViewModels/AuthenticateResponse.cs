﻿using Fanda.Authentication.Repository.Dto;
using System;
using System.Text.Json.Serialization;

namespace Fanda.Authentication.Repository.ViewModels
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse()
        {
        }

        public AuthenticateResponse(UserDto user, Guid tenantId, string jwtToken, string refreshToken,
            bool resetPassword)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            TenantId = tenantId;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            ResetPassword = resetPassword;
        }

        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
        public bool ResetPassword { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}