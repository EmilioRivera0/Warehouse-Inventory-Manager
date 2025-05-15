using System.ComponentModel.DataAnnotations;

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
        // *Only the User Id is stored, since the Users are stored in a different DB Context,
        // storing the User with this model will store a new copy of the user in the model's DB Context
        // generating errors
    }
}
