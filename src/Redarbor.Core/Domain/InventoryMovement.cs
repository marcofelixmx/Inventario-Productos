using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redarbor.Core.Domain
{
    [Table("InventoryMovement")]
    public class InventoryMovement
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
