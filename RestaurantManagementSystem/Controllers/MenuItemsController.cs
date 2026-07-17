using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class MenuItemsController : Controller
    {
        private readonly RestaurantDbContext _context;

        public MenuItemsController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(
            string searchString,
            string sortOrder,
            int? categoryId,
            int? page)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSort = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.SearchString = searchString;

            var items = _context.MenuItems
                .Include(m => m.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                items = items.Where(i =>
                    i.ItemName.Contains(searchString) ||
                    i.Description.Contains(searchString));
            }

            ViewBag.CategoryId = new SelectList(
            _context.Categories,
            "CategoryId",
            "CategoryName",
            categoryId);

            if (categoryId.HasValue)
            {
                items = items.Where(i => i.CategoryId == categoryId);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.ItemName);
                    break;

                case "price":
                    items = items.OrderBy(i => i.Price);
                    break;

                case "price_desc":
                    items = items.OrderByDescending(i => i.Price);
                    break;

                default:
                    items = items.OrderBy(i => i.ItemName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(await items.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(
             _context.Categories,
             "CategoryId",
            "CategoryName");
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,Description,Price,AvailabilityStatus,CategoryId")] MenuItem menuItem)
        {
            ModelState.Remove(nameof(MenuItem.Category));
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "CategoryName",
            menuItem.CategoryId);
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(
             _context.Categories,
            "CategoryId",
            "CategoryName",
            menuItem.CategoryId);
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,Description,Price,AvailabilityStatus,CategoryId")] MenuItem menuItem)
        {
            ModelState.Remove(nameof(MenuItem.Category));

            if (id != menuItem.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "CategoryName",
            menuItem.CategoryId);
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            try
            {
                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Menu item deleted successfully.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] =
                    "Cannot delete this menu item because it exists in previous orders.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.ItemId == id);
        }
    }
}
