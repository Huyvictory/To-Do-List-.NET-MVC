namespace ToDoList.Models;

public class TodoItemsListViewModel
{
    public required IEnumerable<ToDoItemViewModel> ListToDoItems { get; init; }
}