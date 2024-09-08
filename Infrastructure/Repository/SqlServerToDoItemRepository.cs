using Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using UseCases;

namespace Infrastructure.Repository;

public class SqlServerToDoItemRepository : IToDoItemRepository
{
    private readonly ToDoDbContext _context;

    public SqlServerToDoItemRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        return await _context.ToDoItems.ToListAsync();
    }

    public async Task<ToDoItem?> GetByIdAsync(string id)
    {
        return await _context.ToDoItems.FindAsync(id);
    }

    public async Task AddAsync(ToDoItem toDoItem)
    {
        await _context.ToDoItems.AddAsync(toDoItem);
    }

    public async Task UpdateAsync(ToDoItem toDoItem)
    {
        _context.Entry(toDoItem).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(string id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem != null)
        {
            _context.ToDoItems.Remove(toDoItem);
        }
    }
}