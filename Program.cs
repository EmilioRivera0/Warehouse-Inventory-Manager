using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse_Inventory_Manager.Data;
using Warehouse_Inventory_Manager.Models;

var builder = WebApplication.CreateBuilder(args);

// Connect to local SQL Server managed by Visual Studio
/*var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();*/

// Register in-memory database for development stage
// Identity DB
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseInMemoryDatabase("IdentityDB"));
// Models DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("WarehouseInventoryManagerDB"));

// User and Role Registration
builder.Services.AddDefaultIdentity<WarehouseUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Populate DB with test data
// Add Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    List<string> roles = [ "Admin", "Warehouse Staff" ];
    foreach (var role in roles)
        // avoid creating multiple instances when re-running the system
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int>(role));
}
// Add Users
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WarehouseUser>>();

    if (await userManager.FindByEmailAsync("admin@wim.com") == null)
    {
        var adminUser = new WarehouseUser()
        {
            Name = "Mario Rodriguez",
            UserName = "admin@wim.com",
            Email = "admin@wim.com",
            Status = 1,
        };
        var temp = await userManager.CreateAsync(adminUser, "Admin123-");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    } 

    if (await userManager.FindByEmailAsync("worker1@wim.com") == null)
    {
        var workerUser = new WarehouseUser()
        {
            Name = "Pedro Sánchez",
            UserName = "worker1@wim.com",
            Email = "worker1@wim.com",
            Status = 1,
        };
        await userManager.CreateAsync(workerUser, "Worker123-");
        await userManager.AddToRoleAsync(workerUser, "Warehouse Staff");
    }
}
// Add Products
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.ProductsSet.Any())
    {
        Console.WriteLine("Adding products");
        context.ProductsSet.AddRange(
            new Products
            {
                Name = "Hammer",
                Price = 150,
                Stock = 50,
            },
            new Products
            {
                Name = "Nails",
                Price = 5,
                Stock = 1120,
            },
            new Products
            {
                Name = "Jigsaw",
                Price = 500,
                Stock = 2,
                Status = 0,
            }
            );
        context.SaveChanges();
    }
}

app.Run();
