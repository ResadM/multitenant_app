using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace multitenant_app.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string? DbName { get; set; }
    }
}
