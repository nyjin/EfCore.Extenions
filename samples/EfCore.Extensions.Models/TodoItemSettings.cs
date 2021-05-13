using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore.Extensions.Models
{
    public class TodoItemSettings : TrackableEntity
    {
        public string SettingType { get;set; }

        public string SettingKey { get;set; }

        public string SettingValue { get;set; }

        public int TodoItemId { get; set; }

        [ForeignKey(nameof(TodoItemId))]
        public TodoItem TodoItem { get;set; }
    }
}