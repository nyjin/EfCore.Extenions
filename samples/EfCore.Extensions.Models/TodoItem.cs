using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EfCore.Extensions.Models
{
    public class TodoItem : TrackableEntity
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public ICollection<UserTodo> UserTodos { get; set; } = new List<UserTodo>();

        public ICollection<TodoItemSettings> Settings { get;set; } = new List<TodoItemSettings>();

        [NotMapped]
        public ICollection<User> Users => UserTodos.Select(x => x.User).ToList();
    }
}