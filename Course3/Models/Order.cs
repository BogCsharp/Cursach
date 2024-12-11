

using System.ComponentModel.DataAnnotations;

namespace Course3.Models
{
    public class Order
    {
        public int Id { get; set; }

       
        public string UserId { get; set; }

        
        public DateTime OrderDate { get; set; }

       
        public string Status { get; set; }

        public List<OrderItem> Items { get; set; }
        public string PassportNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
      
    }

}
