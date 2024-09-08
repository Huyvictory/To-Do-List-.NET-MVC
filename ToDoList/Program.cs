using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ToDoList.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile("appsettings.secrets.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Bind configuration
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("AppSettings:Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Implement In Memory Unit of Work (InMemoryUnitOfWork)
// builder.Services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();

// Implement Sql Server Unit of Work (SqlServerUnitOfWork)
builder.Services.AddDbContext<ToDoDbContext>((serviceProvider, options) =>
{
    var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
    options.UseSqlServer(appSettings.Database.ConnectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: appSettings.Database.MaxRetryCount,
            maxRetryDelay: TimeSpan.FromSeconds(appSettings.Database.CommandTimeout),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddScoped<IUnitOfWork, SqlServerUnitOfWork>();

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