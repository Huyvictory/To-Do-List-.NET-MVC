namespace ToDoList.Models;

public class ToDoItemViewModel
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
}