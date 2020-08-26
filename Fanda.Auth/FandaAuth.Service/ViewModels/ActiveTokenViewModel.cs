namespace FandaAuth.Service.ViewModels
{
    using System;

    public class ActiveTokenViewModel
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime DateExpires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= DateExpires;
        public DateTime DateCreated { get; set; }
        public string CreatedByIp { get; set; }
    }
}
