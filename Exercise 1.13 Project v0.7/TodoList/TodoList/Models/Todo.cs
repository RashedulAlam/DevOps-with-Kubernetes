using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class Todo
    {
        [MaxLength(140)]
        [Required]
        public string Title { get; set; }

        [MaxLength(520)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set;} = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set;} = DateTime.UtcNow;
    }
}
