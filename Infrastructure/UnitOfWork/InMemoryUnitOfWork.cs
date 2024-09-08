using Infrastructure.Repository;
using UseCases;

namespace Infrastructure.UnitOfWork;

public class InMemoryUnitOfWork : IUnitOfWork
{
    public IToDoItemRepository ToDoItems { get; }

    public InMemoryUnitOfWork()
    {
        ToDoItems = new InMemoryToDoItemRepository();
    }

    public Task<int> CompleteAsync()
    {
        // For in-memory repository, changes are immediate, so we just return a completed task
        return Task.FromResult(1);
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}