using Entities;
using Infrastructure.Repository;

namespace UnitTest.Repository;

public class InMemoryToDoItemRepositoryTests
{
    [Fact]
    public async Task InMemoryToDoItemRepository_GetAllAsync_ReturnsAllItems()
    {
        // Arrange
        var repository = new InMemoryToDoItemRepository();

        // Act
        await repository.AddAsync(new ToDoItem() { Description = "Test Item 1", IsCompleted = false });
        await repository.AddAsync(new ToDoItem() { Description = "Test Item 2", IsCompleted = true });
        var items = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public async Task InMemoryToDoItemRepository_GetByIdAsync_ReturnsItem()
    {
        // Arrange
        var repository = new InMemoryToDoItemRepository();

        // Act
        await repository.AddAsync(new ToDoItem() { Id = "1", Description = "Test Item 1", IsCompleted = false });
        var item = await repository.GetByIdAsync("1");

        // Assert
        Assert.NotNull(item);
        Assert.Equal("1", item!.Id);
    }

    [Fact]
    public async Task InMemoryToDoItemRepository_AddAsync_AddsItem()
    {
        // Arrange
        var repository = new InMemoryToDoItemRepository();

        // Act
        await repository.AddAsync(new ToDoItem() { Description = "Test Item 1", IsCompleted = false });
        var items = await repository.GetAllAsync();

        // Assert
        Assert.Equal("Test Item 1", items.First().Description);
    }

    [Fact]
    public async Task InMemoryToDoItemRepository_UpdateAsync_UpdatesItem()
    {
        // Arrange
        var repository = new InMemoryToDoItemRepository();
        const string mockItemId = "update-id";

        // Act
        await repository.AddAsync(new ToDoItem()
            { Id = mockItemId, Description = "Test Item 1", IsCompleted = false });

        var itemToUpdate = await repository.GetByIdAsync(mockItemId);

        itemToUpdate!.Description = "Updated Item";
        itemToUpdate.IsCompleted = true;

        await repository.UpdateAsync(itemToUpdate);

        var items = await repository.GetAllAsync();

        // Assert
        var toDoItems = items.ToList();
        Assert.Equal("Updated Item", toDoItems.First().Description);
        Assert.True(toDoItems.First().IsCompleted);
    }

    [Fact]
    public async Task InMemoryToDoItemRepository_DeleteAsync_DeletesItem()
    {
        // Arrange
        var repository = new InMemoryToDoItemRepository();
        const string mockItemId = "delete-id";

        // Act
        await repository.AddAsync(new ToDoItem()
            { Id = mockItemId, Description = "Test Item 1", IsCompleted = false });

        await repository.DeleteAsync(mockItemId);

        var items = await repository.GetAllAsync();

        // Assert
        Assert.Empty(items);
    }
}