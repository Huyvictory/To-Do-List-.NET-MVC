using Microsoft.EntityFrameworkCore;
using ToDoList.DbContext;
using ToDoList.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// // Implementation of IToDoItemRepository using local database (InMemoryRepository)
// builder.Services.AddSingleton<IToDoItemRepository, InMemoryToDoItemRepository>();

// Implementation of IToDoItemRepository using EntityFramework and SQL Server database (SqlServerToDoItemRepository)
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IToDoItemRepository, SqlServerToDoItemRepository>();

builder.Services.AddScoped<ToDoListManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();