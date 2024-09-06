using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using UseCases;

namespace ToDoList.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ToDoListManager _toDoListManager;

    public HomeController(ILogger<HomeController> logger, ToDoListManager toDoListManager)
    {
        _logger = logger;
        _toDoListManager = toDoListManager;
    }

    public IActionResult Index()
    {
        var toDoItems = _toDoListManager.GetToDoItemsList();
        return View(new TodoItemsListViewModel
        {
            ListToDoItems = toDoItems.Select(i => new ToDoItemViewModel()
                { Id = i.Id, Description = i.Description, IsCompleted = i.IsCompleted })
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ToDoItemViewModel toDoItemViewModel)
    {
        if (ModelState.IsValid)
        {
            _toDoListManager.AddToDoItem(new ToDoItem()
            {
                Id = 1,
                Description = toDoItemViewModel.Description,
                IsCompleted = false
            });
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}