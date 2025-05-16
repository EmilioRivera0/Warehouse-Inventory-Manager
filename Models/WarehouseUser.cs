using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Warehouse_Inventory_Manager.Models
{
    public class WarehouseUser : IdentityUser<int>
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
        [StringLength(100)]
        public string Name { get; set; } = null!;
        public int Status { get; set; } = 1; // 1 = active (default), 0 = inactive
        // Navigation property to enable relationships
        public List<History> HistoryList { get; set; } = [];
    }
}
