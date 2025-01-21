using Microsoft.EntityFrameworkCore;
using ToDoApi;

var builder = WebApplication.CreateBuilder(args);

//Add dependatcies
builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

// Configure Pipelines
app.MapGet("/todoitems", async (TodoDbContext db) =>
   await db.TodoItems.ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDbContext db) =>
   await db.TodoItems.FindAsync(id));

app.MapPost("/todoitems", async (TodoItem todoItem, TodoDbContext db) =>
{
    db.Add(todoItem);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todoItem.ItemId}", todoItem);
});

app.MapPut("/todoitems", async (int id, TodoItem todoItem, TodoDbContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound($"No todo item found with id: {id}");

    todo.Name = todoItem.Name;
    todo.IsComplete = todoItem.IsComplete;

    //db.Update(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems", async (int id, TodoDbContext db) =>
{
    if (await db.TodoItems.FindAsync(id) is TodoItem todo)
    {
        db.TodoItems.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound($"No todo item found with id: {id}");
});

app.Run();
