using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Product_Id")]
        public long ProductId { get; set; }
        [Column("Name",TypeName ="varchar(200")]
        public string? Name { get; set; }
        //value object
        public ProductDescription? ProductDescription { get; set; }
        public string? SKU { get; set; }

    }
}
