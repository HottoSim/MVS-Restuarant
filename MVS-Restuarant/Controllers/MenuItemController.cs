using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //Edit or update menu Items
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var menuItems = await _context.MenuItems.FindAsync(Id);
            if(menuItems == null)
            {
                return NotFound();
            }
            return View(menuItems);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int Id, [Bind("ItemId,ItemName,ItemDescription,ItemPrice,Category,ImageData,ContentType,IsAvailable")] MenuItem menuItem, IFormFile imageFile)
        {
            if(Id != menuItem.ItemId)
            {
                return NotFound();
            }
            try
            {
                if(imageFile != null && imageFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(stream);
                        menuItem.ImageData = stream.ToArray();
                        menuItem.ContentType = imageFile.ContentType;
                    }
                }
                else
                {
                    //Retain existing image data
                    var existingItem = await _context.MenuItems.AsNoTracking().FirstOrDefaultAsync(m => m.ItemId ==Id);
                    if (existingItem != null)
                    {
                        menuItem.ImageData = existingItem.ImageData;
                        menuItem.ContentType = existingItem.ContentType;
                    }
                }
                _context.Update(menuItem);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, please try again...");
            }
            return RedirectToAction("Index");
        }
    }
}
