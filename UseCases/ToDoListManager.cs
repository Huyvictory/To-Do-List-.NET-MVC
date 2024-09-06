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

    public void AddToDoItem(ToDoItem toDoItem)
    {
        _toDoItemRepository.Add(toDoItem);
    }

    public void MarkCompleted(int id)
    {
        var toDoItem = _toDoItemRepository.GetById(id);
        if (toDoItem != null)
        {
            toDoItem.IsCompleted = true;
            _toDoItemRepository.Update(toDoItem);
        }
    }

    public void Delete(int id)
    {
        var toDoItem = _toDoItemRepository.GetById(id);
        if (toDoItem != null)
        {
            _toDoItemRepository.Delete(id);
        }
    }
}