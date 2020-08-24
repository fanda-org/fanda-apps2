using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core
{
    public class ExistsDto
    {
        [Required]
        public KeyField Field { get; set; }

        [Required]
        public string Value { get; set; }

        public Guid ParentId { get; set; }
    }
}
