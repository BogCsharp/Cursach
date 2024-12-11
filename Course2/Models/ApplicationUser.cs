using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Course2.Models
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
