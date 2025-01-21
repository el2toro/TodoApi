using Microsoft.EntityFrameworkCore;

namespace ToDoApi;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }
}
