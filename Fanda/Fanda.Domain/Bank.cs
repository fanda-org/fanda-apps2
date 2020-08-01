using Fanda.Shared;
using System;

namespace Fanda.Domain
{
    public class Bank
    {
        public Guid LedgerId { get; set; }
        public string AccountNumber { get; set; }
        public BankAccountType AccountType { get; set; }
        public string AccountTypeString
        {
            get { return AccountType.ToString(); }
            set { AccountType = (BankAccountType)Enum.Parse(typeof(BankAccountType), value, true); }
        }
        public string IfscCode { get; set; }
        public string MicrCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? AddressId { get; set; }
        public bool IsDefault { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Address Address { get; set; }
        public virtual Ledger Ledger { get; set; }
    }
}