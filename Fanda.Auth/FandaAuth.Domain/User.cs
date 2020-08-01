using FandaAuth.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FandaAuth.Domain
{
    public class User : UserEntity
    {
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public string PasswordSalt { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateLastLogin { get; set; }

        [JsonIgnore]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
