using Course3.Data;
using Course3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CartController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    private async Task<Cart> GetCartAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return new Cart();
        }

        var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == user.Id);
        if (cart == null)
        {
            cart = new Cart { UserId = user.Id };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    private async Task SaveCartAsync(Cart cart)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return;
        }

        var existingCart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == user.Id);
        if (existingCart != null)
        {
            existingCart.Items = cart.Items;
        }
        else
        {
            cart.UserId = user.Id;
            _context.Carts.Add(cart);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IActionResult> Index()
    {
        var cart = await GetCartAsync();
        return View(cart);
    }


    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, string productName, float price, int quantity = 1, int catalogId = 0)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            return Json(new { success = false, message = "Товар не найден" });
        }

        if (product.Quantity < quantity)
        {
            return Json(new { success = false, message = "Недостаточно товара на складе" });
        }

        var cart = await GetCartAsync();
        var cartItem = new CartItem
        {
            ProductId = productId,
            ProductName = productName,
            Price = price,
            Quantity = quantity
        };

        cart.AddItem(cartItem);
        cart.CatalogId = catalogId;
        await SaveCartAsync(cart);

      
        product.Quantity -= quantity;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Товар добавлен в корзину" });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var cart = await GetCartAsync();
        var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

        if (cartItem != null)
        {
            
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                product.Quantity += cartItem.Quantity;
                await _context.SaveChangesAsync();
            }

            
            cart.RemoveItem(productId);
            await SaveCartAsync(cart);
        }

        return RedirectToAction("Index");
    }
}