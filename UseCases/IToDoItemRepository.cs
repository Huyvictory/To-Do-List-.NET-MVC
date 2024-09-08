using Entities;

namespace UseCases;

public interface IToDoItemRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(string id);
    Task AddAsync(ToDoItem toDoItem);
    Task UpdateAsync(ToDoItem toDoItem);
    Task DeleteAsync(string id);
}