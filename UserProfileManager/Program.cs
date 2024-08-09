using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UserProfileManager.Business.Implementations;
using UserProfileManager.Business.Interfaces;
using UserProfileManager.Data;
using UserProfileManager.Data.Interfaces;
using UserProfileManager.Data.Repositories;
using UserProfileManager.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

builder.Services.AddLogging();
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.LoginPath = "/Auth/Login";
      options.LogoutPath = "/Auth/Logout";
      options.AccessDeniedPath = "/User/AccessDenied";
    });

string? sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServerDefaultConnection");
string? sqliteConnectionString = builder.Configuration.GetConnectionString("SqliteDefaultConnection");

if (sqlServerConnectionString != null)
{
  builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
  dbContextOptions.UseSqlServer(sqlServerConnectionString));
}
else
{
  builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
dbContextOptions.UseSqlite(sqliteConnectionString));
}


builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserBusiness, UserBusiness>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error/ErrorPopup");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "error",
    pattern: "Shared/ErrorPopup",
    defaults: new { controller = "Error", action = "ErrorPopup" });

app.MapGet("/test", () => "Test route is working!");

app.Run();
