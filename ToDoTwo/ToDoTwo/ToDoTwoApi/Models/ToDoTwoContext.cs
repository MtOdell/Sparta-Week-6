using Microsoft.EntityFrameworkCore;

namespace ToDoTwoApi.Models
{
    public class ToDoTwoContext : DbContext
    {
        public ToDoTwoContext(DbContextOptions<ToDoTwoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoTwoItem> TodoTwoItems { get; set; } = null!;
    }
}