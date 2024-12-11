using System.ComponentModel.DataAnnotations;

namespace Course1.Models
{
    public class RegisterViewModel
    { 
    [Required]
        
        [StringLength(30)]
        public string UserName { get; set; }
        [Required, EmailAddress]
        [StringLength(30)]
        public string Email { get; set; }
        public int User_Id { get; set; }
        public int Role_Id { get; set; }
        [Required]
        public int Gender_Id { get; set; }
        [Required]
        [StringLength(30)]
        public string UserLastName { get; set; }
        [Required]
        [StringLength(30)]
        public string UserPatronymic { get; set; }
        [Required]
        public string CityOfBirth { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(30)]
        public string PhoneNumber { get; set; }
    }
}
