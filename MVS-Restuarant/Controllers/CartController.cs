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

        // Display the menu items
        public IActionResult Menu()
        {
            IEnumerable<MenuItem> menuItems = _context.MenuItems.Where(m => m.IsAvailable);
            return View(menuItems);
        }

        // Display the cart
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        public IActionResult AddToCart()
        {
            return View();
        }
        // Add an item to the cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int menuItemId, int quantity = 1)
        {
            var menuItem = await _context.MenuItems.FindAsync(menuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }

            // Retrieve or initialize the cart from session
            var cartItems = GetCartFromSession();

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
                    ItemDescription = menuItem.ItemDescription,
                    Quantity = quantity,
                    ItemPrice = menuItem.ItemPrice,
                    TotalPrice = quantity * menuItem.ItemPrice,
                    ImageData = menuItem.ImageData,
                    ContentType = menuItem.ContentType
                });
            }

            // Save the updated cart back to session
            SaveCartToSession(cartItems);

            // Add success message
            TempData["SuccessMessage"] = $"{menuItem.ItemName} added to your cart.";

            // Redirect to the menu or cart based on preference
            return RedirectToAction("Menu");
        }

        // Remove an item from the cart
        public IActionResult RemoveFromCart(int menuItemId)
        {
            var cartItems = GetCartFromSession();

            // Find and remove the item
            var itemToRemove = cartItems.FirstOrDefault(c => c.MenuItemId == menuItemId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                SaveCartToSession(cartItems);
                TempData["SuccessMessage"] = "Item removed from cart.";
            }

            return RedirectToAction("Index");
        }

        // Update quantity of an item in the cart
        public IActionResult UpdateQuantity(int menuItemId, int quantity)
        {
            if (quantity <= 0)
            {
                return RedirectToAction("RemoveFromCart", new { menuItemId });
            }

            var cartItems = GetCartFromSession();
            var item = cartItems.FirstOrDefault(c => c.MenuItemId == menuItemId);

            if (item != null)
            {
                item.Quantity = quantity;
                item.TotalPrice = quantity * item.ItemPrice;
                SaveCartToSession(cartItems);
            }

            return RedirectToAction("Index");
        }

        // Clear the entire cart
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            TempData["SuccessMessage"] = "Your cart has been cleared.";
            return RedirectToAction("Index");
        }

        // Helper method to get cart from session
        private List<CartItem> GetCartFromSession()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cart)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        // Helper method to save cart to session
        private void SaveCartToSession(List<CartItem> cartItems)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cartItems = GetCartFromSession();
            int count = cartItems.Sum(item => item.Quantity);
            return Json(count);
        }
    }
}
