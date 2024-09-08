using Entities;
using Microsoft.EntityFrameworkCore;
using ToDoList.DbContext;
using ToDoList.Repository;

namespace UnitTest;

public class UnitTestSqlServerRepository : IDisposable
{
    private readonly ToDoDbContext _context;
    private readonly SqlServerToDoItemRepository _repository;

    public UnitTestSqlServerRepository()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ToDoDbContext(options);
        _repository = new SqlServerToDoItemRepository(_context);
    }

    [Fact]
    public void GetAllItems_ReturnsAllItems()
    {
        // Arrange
        _context.ToDoItems.AddRange(
            new ToDoItem { Description = "Task 1" },
            new ToDoItem { Description = "Task 2" },
            new ToDoItem { Description = "Task 3" }
        );
        _context.SaveChanges();

        // Act
        var result = _repository.GetToDoItemsList().ToList();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetItem_ReturnsCorrectItem()
    {
        // Arrange
        var todoItem = new ToDoItem { Description = "Test Task" };
        _context.ToDoItems.Add(todoItem);
        _context.SaveChanges();

        // Act
        var result = _repository.GetById(todoItem.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(todoItem.Description, result.Description);
    }

    [Fact]
    public void AddItem_AddsNewItem()
    {
        // Arrange
        var todoItem = new ToDoItem { Description = "New Task" };

        // Act
        _repository.Add(todoItem);

        // Assert
        var result = _context.ToDoItems.Find(todoItem.Id);
        Assert.NotNull(result);
        Assert.Equal(todoItem.Description, result.Description);
    }

    [Fact]
    public void UpdateItem_UpdatesExistingItem()
    {
        // Arrange
        var todoItem = new ToDoItem { Description = "Original Task" };
        _context.ToDoItems.Add(todoItem);
        _context.SaveChanges();

        // Act
        todoItem.Description = "Updated Task";
        _repository.Update(todoItem);

        // Assert
        var result = _context.ToDoItems.Find(todoItem.Id);
        Assert.NotNull(result);
        Assert.Equal("Updated Task", result.Description);
    }

    [Fact]
    public void DeleteItem_RemovesItem()
    {
        // Arrange
        var todoItem = new ToDoItem { Description = "Task to Delete" };
        _context.ToDoItems.Add(todoItem);
        _context.SaveChanges();

        // Act
        _repository.Delete(todoItem.Id);

        // Assert
        Assert.Null(_context.ToDoItems.Find(todoItem.Id));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}