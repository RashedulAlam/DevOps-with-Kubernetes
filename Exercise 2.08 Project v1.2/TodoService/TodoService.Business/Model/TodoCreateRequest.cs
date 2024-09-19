using System.ComponentModel.DataAnnotations;

namespace TodoService.Business.Model
{
    public class TodoCreateRequest
    {
        [MaxLength(120)]
        public string Title { get; set; }
    }
}
