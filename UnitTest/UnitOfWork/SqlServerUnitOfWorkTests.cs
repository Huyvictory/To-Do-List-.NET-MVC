using Entities;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.UnitOfWork;

public class SqlServerUnitOfWorkTests
{
    private DbContextOptions<ToDoDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task SqlServerUnitOfWork_CompleteAsync_SavesChanges()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var unitOfWork = new SqlServerUnitOfWork(context);

        var todoItem = new ToDoItem() { Description = "Test Item", IsCompleted = false };

        // Act
        await unitOfWork.ToDoItems.AddAsync(todoItem);
        var saveResult = await unitOfWork.CompleteAsync();

        // Assert
        Assert.Equal(1, saveResult);
        Assert.Single(await context.ToDoItems.ToListAsync());
    }

    [Fact]
    public async Task SqlServerUnitOfWork_ToDoItems_ReturnsRepository()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var unitOfWork = new SqlServerUnitOfWork(context);

        // Act
        var repository = unitOfWork.ToDoItems;

        // Assert
        Assert.NotNull(repository);
        Assert.IsType<SqlServerToDoItemRepository>(repository);
    }
}