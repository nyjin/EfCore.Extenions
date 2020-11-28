using System.ComponentModel.DataAnnotations;

namespace EfCore.Extensions.Models
{
    public class User : TrackableEntity
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
    }
}