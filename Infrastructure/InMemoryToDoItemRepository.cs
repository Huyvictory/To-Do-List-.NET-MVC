using Entities;
using UseCases;

namespace Infrastructure;

public class InMemoryToDoItemRepository : IToDoItemRepository
{
    private readonly List<ToDoItem> _items;

    public InMemoryToDoItemRepository(
    )
    {
        _items = new List<ToDoItem>();
    }

    public void Add(ToDoItem item)
    {
        _items.Add(item);
    }

    public void Delete(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public IEnumerable<ToDoItem> GetToDoItemsList()
    {
        return _items;
    }

    public ToDoItem GetById(int id)
    {
        return _items.FirstOrDefault(i => i.Id == id);
    }

    public void Update(ToDoItem item)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Description = item.Description;
            existingItem.IsCompleted = item.IsCompleted;
        }
    }
}