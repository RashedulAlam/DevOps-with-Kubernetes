using TodoService.Business.Model;
using TodoService.Domain.Entity;
using TodoService.Infrastructure.Repository;

namespace TodoService.Business.Services;

public class TodoStorage(IUnitOfWork unitOfWork) : ITodoStorage
{
    private readonly IRepository<Todo> _repository = unitOfWork.GetRepository<Todo>();

    public async Task<TodoDto> Create(TodoCreateRequest todo)
    {
        var newTodo = new Todo
        {
            Title = todo.Title,
        };

        var createdTodo = await this._repository.Add(newTodo);

        return new TodoDto
        {
            Id = createdTodo.Id,
            Title = createdTodo.Title,
        };
    }

    public async Task<IReadOnlyCollection<TodoDto>> GetAll()
    {
        var todos = await this._repository.GetAll();

        return todos.Select(x => new TodoDto
        {
            Title = x.Title,
            Id = x.Id,

        }).ToList();
    }
}