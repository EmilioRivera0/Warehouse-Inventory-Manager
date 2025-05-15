using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse_Inventory_Manager.Models;

namespace Warehouse_Inventory_Manager.Data
{
    public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : IdentityDbContext<WarehouseUser, IdentityRole<int>, int>(options)
    {
    }
}
