using System.Runtime.InteropServices;

namespace Course3.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string Factory { get; set; }
        public string ImageUrl { get; set; }
        public int CatalogId { get; set; }
    }
}
