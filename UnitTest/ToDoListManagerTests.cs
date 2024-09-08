using Entities;
using Moq;
using UseCases;

namespace UnitTest;

public class ToDoListManagerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IToDoItemRepository> _mockRepository;
    private readonly ToDoListManager _manager;

    public ToDoListManagerTests()
    {
        _mockRepository = new Mock<IToDoItemRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.ToDoItems).Returns(_mockRepository.Object);
        _manager = new ToDoListManager(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetToDoItemsList_ReturnsAllItems()
    {
        // Arrange
        var expectedItems = new List<ToDoItem>()
        {
            new() { Id = "1", Description = "Test Item 1", IsCompleted = false },
            new() { Id = "2", Description = "Test Item 2", IsCompleted = true }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedItems);

        // Act
        var result = await _manager.GetToDoItemsList();

        // Assert
        Assert.Equal(expectedItems, result);
    }

    [Fact]
    public async Task AddToDoItem_CallsRepositoryAndCompletes()
    {
        // Arrange
        var toDoItem = new ToDoItem() { Description = "Test Item", IsCompleted = false };

        // Act
        await _manager.AddToDoItem(toDoItem);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(toDoItem), Times.Once);
        _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task ToggleCompleted_UpdatesItemAndCompletes()
    {
        // Arrange
        var toDoItem = new ToDoItem() { Description = "Test Item", IsCompleted = false };
        _mockRepository.Setup(r => r.GetByIdAsync(toDoItem.Id)).ReturnsAsync(toDoItem);

        // Act
        await _manager.ToggleCompleted(toDoItem.Id);

        // Assert
        Assert.True(toDoItem.IsCompleted);
        _mockRepository.Verify(r => r.UpdateAsync(toDoItem), Times.Once);
        _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_ConfirmItemsExistsAndCompletes()
    {
        // Arrange
        var toDoItem = new ToDoItem() { Description = "Test Item", IsCompleted = false };
        _mockRepository.Setup(r => r.GetByIdAsync(toDoItem.Id)).ReturnsAsync(toDoItem);

        // Act
        await _manager.Delete(toDoItem.Id);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(toDoItem.Id), Times.Once);
        _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }
}