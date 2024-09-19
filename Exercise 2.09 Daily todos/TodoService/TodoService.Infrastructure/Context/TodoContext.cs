using Microsoft.EntityFrameworkCore;
using TodoService.Domain.Entity;

namespace TodoService.Infrastructure.Context
{
    public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
    {
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().Property((x => x.Id)).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Todo>().Property((x => x.Title)).IsRequired().HasMaxLength(120);
        }
    }
}
