using TodoService.Model;

namespace TodoService.Services;

public class TodoStorage : ITodoStorage
{
    private readonly ICollection<Todo> _todos = new List<Todo>();

    public Todo Create(Todo todo)
    {
        this._todos.Add(todo);

        return todo;
    }

    public ICollection<Todo> GetAll()
    {
        return this._todos;
    }
}