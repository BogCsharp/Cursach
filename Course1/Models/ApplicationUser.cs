using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Course1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }
        [Required, EmailAddress]
        [StringLength(30)]
        public string Email { get; set; }
    }
}
