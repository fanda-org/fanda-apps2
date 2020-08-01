using System.ComponentModel.DataAnnotations;

namespace Fanda.Core
{
    public class ExistsDto
    {
        [Required]
        public DuplicateField Field { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
