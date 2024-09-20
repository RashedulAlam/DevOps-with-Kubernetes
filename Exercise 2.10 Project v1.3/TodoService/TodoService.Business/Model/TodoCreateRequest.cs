using System.ComponentModel.DataAnnotations;

namespace TodoService.Business.Model
{
    public class TodoCreateRequest
    {
        [MaxLength(140)]
        [Required]
        public string Title { get; set; }
    }
}
