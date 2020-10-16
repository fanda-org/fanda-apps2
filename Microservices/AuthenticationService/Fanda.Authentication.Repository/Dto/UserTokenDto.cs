using System;

namespace Fanda.Authentication.Repository.Dto
{
    public class UserTokenDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime DateExpires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= DateExpires;
        public DateTime DateCreated { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? DateRevoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => DateRevoked == null && !IsExpired;
    }
}
