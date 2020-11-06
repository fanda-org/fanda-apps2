using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string Mobile { get; set; }
        public bool IsPrimary { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<OrgContact> OrgContacts { get; set; }
        // public virtual ICollection<PartyContact> PartyContacts { get; set; }
    }
}