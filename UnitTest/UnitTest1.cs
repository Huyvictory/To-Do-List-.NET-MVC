using Entities;
using Infrastructure;
using UseCases;

namespace UnitTest;

public class UnitTest1
{
    [Fact]
    public void CreateFirstToDoItem_MarkItAsCompleted()
    {
        // Arrange
        // mock the repository
        var mockRepository = new InMemoryToDoItemRepository();
        var toDoListManager = new ToDoListManager(mockRepository);

        var toDoItem = new ToDoItem()
        {
            Id = 1,
            Description = "Test",
            IsCompleted = false
        };

        // Act
        toDoListManager.AddToDoItem(toDoItem);
        toDoListManager.MarkCompleted(1);

        // Assert
        Assert.True(toDoListManager.GetToDoItemsList().First().IsCompleted);
    }
}