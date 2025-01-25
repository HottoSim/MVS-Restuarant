using Microsoft.AspNetCore.Mvc;
using MVS_Restuarant.Data;
using MVS_Restuarant.Models;
using System.Reflection;

namespace MVS_Restuarant.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuItemController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<MenuItem> menuItems = _context.MenuItems;
            return View(menuItems);
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            return View();
        }
        [HttpPost]
        //Create menu items including image upload
        public async Task<IActionResult> CreateItem([Bind("ItemId,ItemName,ItemDescription,ItemPrice,Category,ImageData,ContentType,IsAvailable")] MenuItem menuItem, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImageData", "An image file is required.");
                return View(menuItem); // Return to the view with the validation error
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    menuItem.ImageData = stream.ToArray();
                    menuItem.ContentType = imageFile.ContentType;
                }
            }
            _context.Add(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
