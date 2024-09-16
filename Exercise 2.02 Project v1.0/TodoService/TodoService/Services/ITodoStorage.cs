using TodoService.Model;

namespace TodoService.Services
{
    public interface ITodoStorage
    {
        Todo Create(Todo todo);

        ICollection<Todo> GetAll();
    }
}
