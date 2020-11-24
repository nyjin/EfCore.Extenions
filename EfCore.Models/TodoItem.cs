using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore.Models
{
    public class TodoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;set; }

        [Required]
        [MaxLength(128)]
        public string Name { get;set; }

        public bool IsCompleted { get;set; }
    }
}