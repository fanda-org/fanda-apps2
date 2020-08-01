using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core.Base
{
    public class ParentDuplicate
    {
        [Required]
        public DuplicateField Field { get; set; }

        [Required]
        public string Value { get; set; }

        public Guid Id { get; set; } //= default;
    }

    public class Duplicate : ParentDuplicate
    {
        public Guid ParentId { get; set; } //= default;
    }
}
