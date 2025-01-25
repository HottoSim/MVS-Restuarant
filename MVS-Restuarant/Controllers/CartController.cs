using Microsoft.AspNetCore.Mvc;
using MVS_Restuarant.Data;
using MVS_Restuarant.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace MVS_Restuarant.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Menu
        public IActionResult Menu()
        {
            IEnumerable<MenuItem> menuItems = _context.MenuItems;
            return View(menuItems);
        }

        public List<CartItem> Index()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cart) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        //Add an Item to the cart
        public async Task<IActionResult> AddToCart(int menuItemId, int quantity)
        {
            var menuItem = await _context.MenuItems.FindAsync(menuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }

            // Retrieve or initialize the cart from session
            var cart = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = string.IsNullOrEmpty(cart) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);

            // Check if the item already exists in the cart
            var existingItem = cartItems.FirstOrDefault(c => c.MenuItemId == menuItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.ItemPrice;
            }
            else
            {
                // Add a new item to the cart
                cartItems.Add(new CartItem
                {
                    MenuItemId = menuItem.ItemId,
                    ItemName = menuItem.ItemName,
                    Quantity = quantity,
                    ItemPrice = menuItem.ItemPrice,
                    TotalPrice = quantity * menuItem.ItemPrice
                });
            }

            // Save the updated cart back to session
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

            return RedirectToAction("Index");
        }

        // Remove an item from the cart
        public IActionResult RemoveFromCart(int menuItemId)
        {
            var cart = TempData["Cart"] as List<CartItem> ?? new List<CartItem>();
            cart = cart.Where(c => c.MenuItemId != menuItemId).ToList();
            TempData["Cart"] = cart;
            return RedirectToAction("Index");
        }
    }
}
