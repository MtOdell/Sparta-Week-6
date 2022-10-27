using Microsoft.EntityFrameworkCore;
namespace ToDoApi.Controllers.Models;

public class ToDoItemContext : DbContext
{
    public ToDoItemContext(DbContextOptions<ToDoItemContext>options) : base(options)
    {

    }
    protected ToDoItemContext() { }

    public DbSet<ToDoItem> ToDoItems { get; set; }
}
