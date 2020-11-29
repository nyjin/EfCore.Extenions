using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore.Extensions.Models
{
    public class TrackableEntity : ITrackableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;set; }
        public DateTime CreatedAt { get;set; }
        [Required]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get;set; }
        [Required]
        public string UpdatedBy { get;set; }
    }
}