using Microsoft.AspNetCore.Identity;

namespace Warehouse_Inventory_Manager.Models
{
    public class Products
    {
        // By convention the PK should be called Id instead of idProduct
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; } = 0;
        public int Status { get; set; } = 1; // 1 = active (default), 0 = inactive
        // Navigation property to enable relationships
        public List<History> HistoryList { get; set; } = [];
    }
}
