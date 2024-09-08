using Entities;
using UseCases;

namespace Infrastructure.Repository;

public class InMemoryToDoItemRepository : IToDoItemRepository
{
    private readonly List<ToDoItem> _items = new List<ToDoItem>();

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        return await Task.FromResult(_items);
    }

    public async Task<ToDoItem?> GetByIdAsync(string id)
    {
        return await Task.FromResult(_items.FirstOrDefault(i => i.Id == id));
    }

    public async Task AddAsync(ToDoItem toDoItem)
    {
        _items.Add(toDoItem);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(ToDoItem toDoItem)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == toDoItem.Id);
        if (existingItem != null)
        {
            existingItem.Description = toDoItem.Description;
            existingItem.IsCompleted = toDoItem.IsCompleted;
        }

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(string id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            _items.Remove(item);
        }

        await Task.CompletedTask;
    }
}