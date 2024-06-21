using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDoApp.Models
{
    public class ToDo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; } 

        [Required]
        public PriorityLevel Priority { get; set; } // "High", "Medium", "Low"

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; } // "Work", "Personal"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
