using Entities;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Repository;

public class SqlServerToDoItemRepositoryTests
{
    private DbContextOptions<ToDoDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task SqlServerToDoItemRepository_GetAllAsync_ReturnsAllItems()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var repository = new SqlServerToDoItemRepository(context);

        // Act
        await context.ToDoItems.AddAsync(new ToDoItem() { Description = "Test Item 1", IsCompleted = false });
        await context.ToDoItems.AddAsync(new ToDoItem() { Description = "Test Item 2", IsCompleted = true });
        await context.SaveChangesAsync();
        var items = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public async Task SqlServerToDoItemRepository_GetByIdAsync_ReturnsItem()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var repository = new SqlServerToDoItemRepository(context);

        // Act
        await context.ToDoItems.AddAsync(new ToDoItem() { Id = "1", Description = "Test Item 1", IsCompleted = false });
        await context.SaveChangesAsync();
        var item = await repository.GetByIdAsync("1");

        // Assert
        Assert.NotNull(item);
        Assert.Equal("1", item!.Id);
    }

    [Fact]
    public async Task SqlServerToDoItemRepository_AddAsync_AddsItem()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var repository = new SqlServerToDoItemRepository(context);

        // Act
        await repository.AddAsync(new ToDoItem() { Description = "Test Item 1", IsCompleted = false });
        await context.SaveChangesAsync();
        var items = await context.ToDoItems.ToListAsync();

        // Assert
        Assert.Equal("Test Item 1", items.First().Description);
    }

    [Fact]
    public async Task SqlServerToDoItemRepository_UpdateAsync_UpdatesItem()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var repository = new SqlServerToDoItemRepository(context);
        const string mockItemId = "update-id";

        // Act
        await context.ToDoItems.AddAsync(new ToDoItem()
            { Id = mockItemId, Description = "Test Item 1", IsCompleted = false });
        await context.SaveChangesAsync();

        var itemToUpdate = await repository.GetByIdAsync(mockItemId);

        itemToUpdate!.Description = "Updated Item";
        itemToUpdate.IsCompleted = true;

        await repository.UpdateAsync(itemToUpdate);
        await context.SaveChangesAsync();

        var items = await repository.GetAllAsync();

        // Assert
        var toDoItems = items.ToList();
        Assert.Equal("Updated Item", toDoItems.First().Description);
        Assert.True(toDoItems.First().IsCompleted);
    }

    [Fact]
    public async Task SqlServerToDoItemRepository_DeleteAsync_DeletesItem()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using var context = new ToDoDbContext(options);
        var repository = new SqlServerToDoItemRepository(context);
        const string mockItemId = "delete-id";

        // Act
        await context.ToDoItems.AddAsync(new ToDoItem()
            { Id = mockItemId, Description = "Test Item 1", IsCompleted = false });
        await context.SaveChangesAsync();

        await repository.DeleteAsync(mockItemId);
        await context.SaveChangesAsync();

        var items = await repository.GetAllAsync();

        // Assert
        Assert.Empty(items);
    }
}