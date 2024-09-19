using TodoService.Business.Model;

namespace TodoService.Business.Services
{
    public interface ITodoStorage
    {
        Task<TodoDto> Create(TodoCreateRequest todo);

        Task<IReadOnlyCollection<TodoDto>> GetAll();
    }
}
