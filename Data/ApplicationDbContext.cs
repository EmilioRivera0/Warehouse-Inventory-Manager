using Microsoft.EntityFrameworkCore;
using Warehouse_Inventory_Manager.Models;

namespace Warehouse_Inventory_Manager.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Products> ProductsSet { get; set; }
        public DbSet<History> HistorySet { get; set; }
    }
}
