using Entities;
using Microsoft.Extensions.Logging;

namespace UseCases;

public class ToDoListManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ToDoListManager> _logger;

    public ToDoListManager(IUnitOfWork unitOfWork, ILogger<ToDoListManager> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<ToDoItem>> GetToDoItemsList()
    {
        _logger.LogInformation("Fetching all ToDo items");
        return await _unitOfWork.ToDoItems.GetAllAsync();
    }

    public async Task<ToDoItem?> GetById(string id)
    {
        _logger.LogInformation("Fetching ToDo item with ID: {Id}", id);
        return await _unitOfWork.ToDoItems.GetByIdAsync(id);
    }

    public async Task AddToDoItem(ToDoItem toDoItem)
    {
        _logger.LogInformation("Adding new ToDo item: {@ToDoItem}", toDoItem);
        await _unitOfWork.ToDoItems.AddAsync(toDoItem);
        await _unitOfWork.CompleteAsync();
    }

    public async Task ToggleCompleted(string id)
    {
        _logger.LogInformation("Toggling completed status of ToDo item with ID: {Id}", id);
        var toDoItem = await _unitOfWork.ToDoItems.GetByIdAsync(id);
        if (toDoItem != null)
        {
            toDoItem.IsCompleted = !toDoItem.IsCompleted;
            await _unitOfWork.ToDoItems.UpdateAsync(toDoItem);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task Update(ToDoItem toDoItem)
    {
        _logger.LogInformation("Updating ToDo item: {@ToDoItem}", toDoItem);
        await _unitOfWork.ToDoItems.UpdateAsync(toDoItem);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(string id)
    {
        _logger.LogInformation("Deleting ToDo item with ID: {Id}", id);
        var toDoItem = await _unitOfWork.ToDoItems.GetByIdAsync(id);
        if (toDoItem != null)
        {
            await _unitOfWork.ToDoItems.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}