using Entities;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;

namespace UnitTest.UnitOfWork;

public class InMemoryUnitOfWorkTests
{
    [Fact]
    public async Task InMemoryUnitOfWork_CompleteAsync_ReturnsOne()
    {
        // Arrange
        var unitOfWork = new InMemoryUnitOfWork();

        // Act
        var result = await unitOfWork.CompleteAsync();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task InMemoryUnitOfWork_ToDoItems_ReturnsRepository()
    {
        // Arrange
        var unitOfWork = new InMemoryUnitOfWork();

        // Act
        var repository = unitOfWork.ToDoItems;

        // Assert
        Assert.NotNull(repository);
        Assert.IsType<InMemoryToDoItemRepository>(repository);
    }

    [Fact]
    public async Task InMemoryUnitOfWork_AddAndRetrieveItem_WorksCorrectly()
    {
        // Arrange
        var unitOfWork = new InMemoryUnitOfWork();
        var todoItem = new ToDoItem() { Description = "Test Item", IsCompleted = false };

        // Act
        await unitOfWork.ToDoItems.AddAsync(todoItem);
        await unitOfWork.CompleteAsync();
        var retrievedItems = await unitOfWork.ToDoItems.GetAllAsync();

        // Assert
        var toDoItems = retrievedItems.ToList();
        Assert.Single(toDoItems);
        Assert.Equal(todoItem.Description, toDoItems.First().Description);
    }
}