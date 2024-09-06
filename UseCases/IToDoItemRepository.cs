using Entities;

namespace UseCases;

public interface IToDoItemRepository
{
    IEnumerable<ToDoItem> GetToDoItemsList();
    ToDoItem? GetById(int id);
    void Add(ToDoItem toDoItem);
    void Update(ToDoItem toDoItem);
    void Delete(int id);
}