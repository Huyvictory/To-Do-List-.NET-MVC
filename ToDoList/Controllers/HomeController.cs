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

    public async Task<IActionResult> Index()
    {
        var toDoItems = await _toDoListManager.GetToDoItemsList();
        _logger.LogInformation($"Retrieved {toDoItems.Count()} items");
        return View(new TodoItemsListViewModel
        {
            ListToDoItems = toDoItems.Select(i => new ToDoItemViewModel()
                { Id = i.Id, Description = i.Description, IsCompleted = i.IsCompleted }).ToList()
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItemViewModel toDoItemViewModel)
    {
        if (ModelState.IsValid)
        {
            await _toDoListManager.AddToDoItem(new ToDoItem()
            {
                Id = toDoItemViewModel.Id,
                Description = toDoItemViewModel.Description,
                IsCompleted = false
            });
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var todoItem = await _toDoListManager.GetById(id);
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
    public async Task<IActionResult> Edit(string id, [Bind("Id,Description,IsCompleted")] ToDoItemViewModel todoItem)
    {
        if (id != todoItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _toDoListManager.Update(new ToDoItem()
                {
                    Id = todoItem.Id,
                    Description = todoItem.Description,
                    IsCompleted = todoItem.IsCompleted
                });
            }
            catch (Exception)
            {
                if (await _toDoListManager.GetById(todoItem.Id) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(todoItem);
    }

    public async Task<IActionResult> ToggleComplete(string id)
    {
        await _toDoListManager.ToggleCompleted(id);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _toDoListManager.Delete(id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}