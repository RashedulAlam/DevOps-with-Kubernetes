using System.ComponentModel.DataAnnotations;

namespace TodoService.Model
{
    public class Todo
    {
        [MaxLength(120)]
        public string Title { get; set; }
    }
}
