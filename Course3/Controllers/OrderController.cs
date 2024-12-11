using Course3.Data;
using Course3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> CreateOrderForm()
    {
        var user = await _userManager.GetUserAsync(User);
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var order = new Order
        {
            UserId = user.Id,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            Items = cart.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrderForm(Order order)
    {
        var user = await _userManager.GetUserAsync(User);
        order.UserId = user.Id;
        order.OrderDate = DateTime.UtcNow;
        order.Status = "Pending";

        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); 

        
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == user.Id);
        if (cart != null)
        {
            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id, 
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                _context.OrderItems.Add(orderItem); 
            }
            await _context.SaveChangesAsync(); 
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync(); 
        }

        return RedirectToAction("MyOrders", "Order");
    }


    [HttpGet]
    public async Task<IActionResult> EditOrder(int id)
    {
        var order = await _context.Orders
     .Include(o => o.Items) 
     .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (order.UserId != user.Id)
        {
            return Forbid();
        }

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOrder(int id, Order order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        var existingOrder = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (existingOrder == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (existingOrder.UserId != user.Id)
        {
            return Forbid();
        }

    
        

        
        existingOrder.ClientName = order.ClientName;
        existingOrder.PassportNumber = order.PassportNumber;
        existingOrder.Address = order.Address; 
        existingOrder.Number = order.Number;
        existingOrder.Email = order.Email;

        _context.Orders.Update(existingOrder);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyOrders", "Order");
    }




    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        var order = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == user.Id)
            .ToListAsync();

        return View(order);
    }


    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

        if (order == null || order.UserId != (await _userManager.GetUserAsync(User)).Id)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost, ActionName("DeleteOrderConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOrderConfirmed(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("MyOrders");
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items) 
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (order.UserId != user.Id)
        {
            return Forbid();
        }

        return View(order);
    }
    [HttpPost]
    public IActionResult ChangeStatus(int id, string status)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.Status = status;
            _context.SaveChanges();
        }
        return RedirectToAction("AllOrders","Home"); 
    }


}
