using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

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
                Id = toDoItemViewModel.Id,
                Description = toDoItemViewModel.Description,
                IsCompleted = false
            });
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoItem = _toDoListManager.GetById(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        return View(new ToDoItemViewModel()
        {
            Id = todoItem.Id,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted
        });
    }

    // POST: Home/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(string id, [Bind("Id,Description,IsCompleted")] ToDoItemViewModel todoItem)
    {
        if (id != todoItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _toDoListManager.Update(new ToDoItem()
                {
                    Id = todoItem.Id,
                    Description = todoItem.Description,
                    IsCompleted = todoItem.IsCompleted
                });
            }
            catch (Exception)
            {
                if (_toDoListManager.GetById(todoItem.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(todoItem);
    }

    public IActionResult ToggleComplete(string id)
    {
        _toDoListManager.ToggleCompleted(id);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        _toDoListManager.Delete(id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}