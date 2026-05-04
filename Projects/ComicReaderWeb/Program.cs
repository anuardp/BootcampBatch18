using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using ComicReader.Data;
using ComicReader.Models;
using ComicReader.Services;
using ComicReader.Repositories;
using ComicReader.MappingProfiles;
using ComicReader.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add password hasher for Customer (manual hashing)
builder.Services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

// Configure authentication cookie (manual, no Identity)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ComicReaderProfile));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>(); // salah satu validator

// Add Repository Layer
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IComicRepository, ComicRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<ISubscribeHistoryRepository, SubscribeHistoryRepository>();

// Add Service Layer
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IComicService, ComicService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ISubscribeHistoryService, SubscribeHistoryService>();

// Add MVC Controllers with Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed database with default users (admin, free, subscribed)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default route (home controller)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Admin area route (optional, can be handled by default controller)
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.Run();