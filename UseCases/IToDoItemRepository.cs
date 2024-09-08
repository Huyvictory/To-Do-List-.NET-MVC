using Entities;

namespace UseCases;

public interface IToDoItemRepository
{
    IEnumerable<ToDoItem> GetToDoItemsList();
    ToDoItem? GetById(string id);
    void Add(ToDoItem toDoItem);
    void Update(ToDoItem toDoItem);
    void Delete(string id);
}