using Microsoft.AspNetCore.Identity;

namespace Warehouse_Inventory_Manager.Models
{
    public class History
    {
        // By convention the PK should be called Id instead of idHistory
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public DateTime Datetime { get; set; } = DateTime.Now;
        // FK
        public int IdProduct { get; set; }
        public Products Product { get; set; } = null!;
        // FK
        public int IdUser { get; set; }
        public WarehouseUser WarehouseUser { get; set; } = null!;
    }
}
