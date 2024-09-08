using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using UseCases;

namespace Infrastructure.UnitOfWork;

public class SqlServerUnitOfWork : IUnitOfWork
{
    private readonly ToDoDbContext _context;

    private readonly ILogger _logger;
    public IToDoItemRepository ToDoItems { get; }

    public SqlServerUnitOfWork(ToDoDbContext context, ILogger<SqlServerUnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
        ToDoItems = new SqlServerToDoItemRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        _logger.LogInformation("Saving changes to the database");
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing SqlServerUnitOfWork");
        await _context.DisposeAsync();
    }
}