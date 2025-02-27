using System.ComponentModel.DataAnnotations;

namespace multitenant_app.Models.UserModels
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string ProductCode { get; set; }

        [Required]
        [MaxLength(100)]
        public required string ProductName { get; set; }
        public double ProductPrice { get; set; } 
    }
}
