using Microsoft.AspNetCore.Mvc;
using MVS_Restuarant.Data;
using MVS_Restuarant.Migrations;
using MVS_Restuarant.Models;
using Newtonsoft.Json;

namespace MVS_Restuarant.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show Checkout page
        [HttpGet]
        public IActionResult Checkout()
        {
            var cartItems = GetCartFromSession();
            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            var orderTotal = cartItems.Sum(item => item.TotalPrice) + 30; // +30 delivery fee
            ViewBag.OrderTotal = orderTotal;

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {
            var cartItems = GetCartFromSession();
            if (cartItems == null || cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your Cart is empty";
                return RedirectToAction("Menu", "Cart");
            }

            //Calculate cart total
            decimal cartTotal = cartItems.Sum(item => item.TotalPrice);


            //Create order
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = cartTotal + 30,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            // Create OrderItems
            foreach(var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    ItemPrice = item.ItemPrice,
                };
                order.OrderItems.Add(orderItem);
            }

            // Save order to database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the cart after placing order
            HttpContext.Session.Remove("Cart");

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("Index","Cart");
        }

        //Order Confirmationn
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // Helper method to get cart from session
        private List<CartItem> GetCartFromSession()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cart)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }
    }
}
