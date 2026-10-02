using DotNetApp.Components;
using DotNetApp.Data;
using DotNetApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// EF Core DbContext Factory for Blazor Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Data Source=novapulse.db";

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Application Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ToastService>();

var app = builder.Build();

// Ensure DB is created & seeded on startup
using (var scope = app.Services.CreateScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var dbContext = contextFactory.CreateDbContext();
    await DbInitializer.InitializeAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
