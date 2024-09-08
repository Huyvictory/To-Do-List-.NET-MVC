using Infrastructure.Data;
using Infrastructure.Repository;
using UseCases;

namespace Infrastructure.UnitOfWork;

public class SqlServerUnitOfWork : IUnitOfWork
{
    private readonly ToDoDbContext _context;
    public IToDoItemRepository ToDoItems { get; }

    public SqlServerUnitOfWork(ToDoDbContext context)
    {
        _context = context;
        ToDoItems = new SqlServerToDoItemRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}