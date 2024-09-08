using ToDoList.DbContext;

namespace ToDoList.Repository;

public class SqlServerToDoItemRepository : IToDoItemRepository
{
    private readonly ToDoDbContext _context;

    public SqlServerToDoItemRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public IEnumerable<ToDoItem> GetToDoItemsList()
    {
        return _context.ToDoItems.ToList();
    }

    public ToDoItem? GetById(string id)
    {
        return _context.ToDoItems.Find(id);
    }

    public void Add(ToDoItem toDoItem)
    {
        _context.ToDoItems.Add(toDoItem);
        _context.SaveChanges();
    }

    public void Update(ToDoItem toDoItem)
    {
        _context.ToDoItems.Update(toDoItem);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var toDoItem = _context.ToDoItems.Find(id);
        if (toDoItem != null)
        {
            _context.ToDoItems.Remove(toDoItem);
            _context.SaveChanges();
        }
    }
}