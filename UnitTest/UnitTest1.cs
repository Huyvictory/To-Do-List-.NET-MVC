using Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoList.Controllers;
using ToDoList.Models;
using UseCases;

namespace UnitTest;

public class UnitTest1
{
    [Fact]
    public void Index_ReturnsViewWithListOfTodoItems()
    {
        // Arrange

        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockToDoRepository = new Mock<IToDoItemRepository>();
        var toDoListManager = new ToDoListManager(mockToDoRepository.Object);
        var expectedItems = new List<ToDoItem>
        {
            new ToDoItem { Id = "1", Description = "Task 1", IsCompleted = false },
            new ToDoItem { Id = "2", Description = "Task 2", IsCompleted = true },
            new ToDoItem { Id = "3", Description = "Task 3", IsCompleted = false }
        };
        mockToDoRepository.Setup(service => service.GetToDoItemsList()).Returns(expectedItems);

        var controller = new HomeController(mockLogger.Object, toDoListManager);

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<TodoItemsListViewModel>(result.Model);
        var toDoItemViewModels = model.ListToDoItems.ToList();
        Assert.Equal(3, toDoItemViewModels.Count);
    }

    [Fact]
    public void Index_ReturnsEmptyList_WhenNoItemsExist()
    {
        // Arrange
        var mockToDoRepository = new Mock<IToDoItemRepository>();
        var mockLogger = new Mock<ILogger<HomeController>>();
        var toDoListManager = new ToDoListManager(mockToDoRepository.Object);
        mockToDoRepository.Setup(service => service.GetToDoItemsList()).Returns(new List<ToDoItem>());

        var controller = new HomeController(mockLogger.Object, toDoListManager);

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<TodoItemsListViewModel>(result.Model);
        Assert.Empty(model.ListToDoItems);
    }

    // Test Create ToDoItem
    [Fact]
    public void TestCreateToDoItem()
    {
        // Arrange
        // mock the repository
        var mockRepository = new InMemoryToDoItemRepository();
        var toDoListManager = new ToDoListManager(mockRepository);

        // example guid for testing
        var toDoItemGuidTest = Guid.NewGuid().ToString();

        var toDoItem = new ToDoItem()
        {
            Id = toDoItemGuidTest,
            Description = "Test",
            IsCompleted = false
        };

        // Act
        toDoListManager.AddToDoItem(toDoItem);

        // Assert
        Assert.Equal(toDoItem.Id, toDoListManager.GetToDoItemsList().First().Id);
    }

    // Test for toggle function
    [Fact]
    public void TestToggleToDoItem()
    {
        // Arrange
        // mock the repository
        var mockRepository = new InMemoryToDoItemRepository();
        var toDoListManager = new ToDoListManager(mockRepository);

        // example guid for testing
        var toDoItemGuidTest = Guid.NewGuid().ToString();

        var toDoItem = new ToDoItem()
        {
            Id = toDoItemGuidTest,
            Description = "Test",
            IsCompleted = false
        };

        // Act
        toDoListManager.AddToDoItem(toDoItem);
        toDoListManager.ToggleCompleted(toDoItemGuidTest);

        // Assert
        Assert.True(toDoListManager.GetToDoItemsList().First().IsCompleted);

        toDoListManager.ToggleCompleted(toDoItemGuidTest);
        Assert.False(toDoListManager.GetToDoItemsList().First().IsCompleted);
    }

    // Test for update function
    [Fact]
    public void TestUpdateToDoItem()
    {
        // Arrange
        // mock the repository
        var mockRepository = new InMemoryToDoItemRepository();
        var toDoListManager = new ToDoListManager(mockRepository);

        // example guid for testing
        var toDoItemGuidTest = Guid.NewGuid().ToString();

        var toDoItemTest = new ToDoItem()
        {
            Id = toDoItemGuidTest,
            Description = "Test",
            IsCompleted = false
        };

        // Act
        toDoListManager.AddToDoItem(toDoItemTest);

        toDoListManager.Update(new ToDoItem()
        {
            Id = toDoItemGuidTest,
            Description = "Test Updated",
            IsCompleted = true
        });

        Assert.Equal("Test Updated", toDoListManager.GetToDoItemsList().First().Description);
    }

    //Test for delete function
    [Fact]
    public void TestDeleteToDoItem()
    {
        // Arrange
        // mock the repository
        var mockRepository = new InMemoryToDoItemRepository();
        var toDoListManager = new ToDoListManager(mockRepository);

        // example guid for testing
        var toDoItemGuidTest = Guid.NewGuid().ToString();

        var toDoItemTest = new ToDoItem()
        {
            Id = toDoItemGuidTest,
            Description = "Test",
            IsCompleted = false
        };

        // Act
        toDoListManager.AddToDoItem(toDoItemTest);
        toDoListManager.Delete(toDoItemGuidTest);

        Assert.Null(toDoListManager.GetById(toDoItemGuidTest));
    }
}