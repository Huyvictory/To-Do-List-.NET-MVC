namespace Entities;

public class ToDoItem
{
    public string Id { get; set; }
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
}