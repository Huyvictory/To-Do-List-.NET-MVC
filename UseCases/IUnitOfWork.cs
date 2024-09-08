namespace UseCases;

public interface IUnitOfWork : IAsyncDisposable
{
    IToDoItemRepository ToDoItems { get; }
    Task<int> CompleteAsync();
}