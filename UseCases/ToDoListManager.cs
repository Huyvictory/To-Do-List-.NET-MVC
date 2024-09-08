using Entities;

namespace UseCases;

public class ToDoListManager
{
    private readonly IToDoItemRepository _toDoItemRepository;

    public ToDoListManager(IToDoItemRepository toDoItemRepository)
    {
        _toDoItemRepository = toDoItemRepository;
    }

    public IEnumerable<ToDoItem> GetToDoItemsList()
    {
        return _toDoItemRepository.GetToDoItemsList();
    }

    public ToDoItem? GetById(string id)
    {
        return _toDoItemRepository.GetById(id);
    }

    public void AddToDoItem(ToDoItem toDoItem)
    {
        _toDoItemRepository.Add(toDoItem);
    }

    public void ToggleCompleted(string id)
    {
        var toDoItem = _toDoItemRepository.GetById(id);
        if (toDoItem != null)
        {
            toDoItem.IsCompleted = !toDoItem.IsCompleted;
            _toDoItemRepository.Update(toDoItem);
        }
    }

    public void Update(ToDoItem toDoItem)
    {
        _toDoItemRepository.Update(toDoItem);
    }

    public void Delete(string id)
    {
        var toDoItem = _toDoItemRepository.GetById(id);
        if (toDoItem != null)
        {
            _toDoItemRepository.Delete(id);
        }
    }
}