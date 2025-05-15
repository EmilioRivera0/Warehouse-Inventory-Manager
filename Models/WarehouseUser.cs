using Microsoft.AspNetCore.Identity;

namespace Warehouse_Inventory_Manager.Models
{
    public class WarehouseUser : IdentityUser
    {
        /* IdentityUser class already implements:
         * - idUser
         * - email
         * - password (hashed)
         * - idRol (handles relationship with Roles)
         */
        /* IdentityRole class already implements:
         * - idRol
         * - name
         */
        public string Name { get; set; } = null!;
        public int Status { get; set; } = 1; // 1 = active (default), 0 = inactive
        // Navigation property to enable relationships
        public List<History> HistoryList { get; set; } = [];
    }
}
