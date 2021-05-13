using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore.Extensions.Models
{
    public class UserTodo : TrackableEntity
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int TodoId { get; set; }

        [ForeignKey(nameof(TodoId))]
        public TodoItem Todo { get; set; }
    }
}