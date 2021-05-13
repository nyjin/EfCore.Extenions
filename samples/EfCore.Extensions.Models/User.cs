using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EfCore.Extensions.Models
{
    public class User : TrackableEntity
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public ICollection<UserTodo> UserTodos { get; set; }

        [NotMapped]
        public ICollection<TodoItem> Todos => UserTodos.Select(x => x.Todo).ToList();
    }
}