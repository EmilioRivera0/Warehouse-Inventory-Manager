using Microsoft.AspNetCore.Identity;

namespace Warehouse_Inventory_Manager.Models
{
    public class History
    {
        public int IdHistory { get; set; }
        public int IdProduct { get; set; }
        public Products Product { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int IdUser { get; set; }
        public WarehouseUser WarehouseUser { get; set; } = null!;
        public DateTime Datetime { get; set; } = DateTime.Now;
    }
}
