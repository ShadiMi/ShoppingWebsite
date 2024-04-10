using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingWebsite.Areas.Identity.Data;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext for use with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure ASP.NET Core Identity to use ShoppingWebsiteUser and include roles
builder.Services.AddDefaultIdentity<ShoppingWebsiteUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Add role services here
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services for session handling
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout duration
    options.Cookie.HttpOnly = true; // Prevent access to session cookie from client scripts
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enables authentication capabilities
app.UseAuthorization(); // Enables authorization capabilities

app.UseSession(); // Add session middleware here, after UseRouting and before UseEndpoints

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create a scope to get scoped services
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ShoppingWebsiteUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure the database is migrated
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        // Method calls to ensure the Admin role exists and assign "shadi@gmail.com" to it
        await EnsureAdminRoleExists(roleManager);
        await EnsureUserIsAdmin(userManager, "shadi@gmail.com");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while setting up roles and admin user.");
    }
}

app.Run();

async Task EnsureAdminRoleExists(RoleManager<IdentityRole> roleManager)
{
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
}

async Task EnsureUserIsAdmin(UserManager<ShoppingWebsiteUser> userManager, string email)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user != null && !(await userManager.IsInRoleAsync(user, "Admin")))
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
