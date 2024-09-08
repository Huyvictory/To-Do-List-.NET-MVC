namespace ToDoList.Models;

public class ToDoItemViewModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
}