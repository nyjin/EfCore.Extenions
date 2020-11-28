using System.ComponentModel.DataAnnotations;

namespace EfCore.Extensions.Models
{
    public class TodoItem : TrackableEntity
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsCompleted { get; set; }
    }
}