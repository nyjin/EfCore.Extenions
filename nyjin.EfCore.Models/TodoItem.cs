using System.ComponentModel.DataAnnotations;

namespace nyjin.EfCore.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get;set; }

        [Required]
        [MaxLength(128)]
        public string Name { get;set; }

        public bool IsCompleted { get;set; }
    }
}