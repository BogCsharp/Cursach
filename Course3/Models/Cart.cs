namespace Course3.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CatalogId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
            }
        }
    }
}
