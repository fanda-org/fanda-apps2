using System;

namespace Fanda.Core.Base
{
    //public class RootEntityBase
    //{
    //    public Guid Id { get; set; }
    //    public bool Active { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public DateTime? DateModified { get; set; }
    //}

    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class BaseOrgEntity : BaseEntity
    {
        public Guid OrgId { get; set; }
    }

    public class BaseYearEntity
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Guid YearId { get; set; }
    }
}
