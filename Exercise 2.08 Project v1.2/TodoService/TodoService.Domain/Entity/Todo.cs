using TodoService.Domain.Common;

namespace TodoService.Domain.Entity
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }
    }
}
