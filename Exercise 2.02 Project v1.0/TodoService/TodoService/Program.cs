using Microsoft.AspNetCore.Mvc;
using TodoService.Model;
using TodoService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITodoStorage, TodoStorage>();

var app = builder.Build();

app.UsePathBase("/todo-service");
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/todos", (ITodoStorage todoStorage) => todoStorage.GetAll())
.WithName("GetTodos")
.WithOpenApi();

app.MapPost("/todos", ([FromBody] Todo todo, ITodoStorage todoStorage) =>
    { 
        var newTodo = todoStorage.Create(todo); 

        return newTodo;
})
.WithName("CreateTodos")
.WithOpenApi();

app.Run();
