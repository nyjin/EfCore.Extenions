using System;

namespace EfCore.Extensions.Models
{
    public interface ITrackableEntity : IEntity
    {
        public DateTime CreatedAt { get;set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get;set; }
        public string UpdatedBy { get;set; }

    }
}