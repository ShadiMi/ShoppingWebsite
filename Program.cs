using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data; // Replace 'YourNamespace' with the actual namespace of your ApplicationDbContext
using Microsoft.AspNetCore.Identity;
using ShoppingWebsite.Models; // Replace 'YourNamespace' with your models' namespace, if you're using custom IdentityUser
using ShoppingWebsite.Repository; // Replace 'YourNamespace' with your repository namespace, if applicable
using ShoppingWebsite.Services; // Replace 'YourNamespace' with your services namespace, if applicable

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
// Configure DbContext for use with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// If you have additional services and repositories, register them here
// Example:
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
// builder.Services.AddScoped<CustomerService>();

// Add services for MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enables authentication capabilities
app.UseAuthorization(); // Enables authorization capabilities

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
