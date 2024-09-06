namespace Entities;

public class ToDoItem
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
}