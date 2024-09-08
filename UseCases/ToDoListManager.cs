using Entities;

namespace UseCases;

public class ToDoListManager
{
    private readonly IUnitOfWork _unitOfWork;

    public ToDoListManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ToDoItem>> GetToDoItemsList()
    {
        return await _unitOfWork.ToDoItems.GetAllAsync();
    }

    public async Task<ToDoItem?> GetById(string id)
    {
        return await _unitOfWork.ToDoItems.GetByIdAsync(id);
    }

    public async Task AddToDoItem(ToDoItem toDoItem)
    {
        await _unitOfWork.ToDoItems.AddAsync(toDoItem);
        await _unitOfWork.CompleteAsync();
    }

    public async Task ToggleCompleted(string id)
    {
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
        await _unitOfWork.ToDoItems.UpdateAsync(toDoItem);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(string id)
    {
        var toDoItem = await _unitOfWork.ToDoItems.GetByIdAsync(id);
        if (toDoItem != null)
        {
            await _unitOfWork.ToDoItems.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}