using Microsoft.EntityFrameworkCore;

namespace ToDoList.DbContext;

public class ToDoDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired().HasMaxLength(36);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
        });
    }
}