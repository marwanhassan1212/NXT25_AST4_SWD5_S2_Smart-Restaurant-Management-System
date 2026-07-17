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
    [Authorize(Roles = "Admin,Manager,Waiter")]
    public class RestaurantTablesController : Controller
    {
        private readonly RestaurantDbContext _context;

        public RestaurantTablesController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: RestaurantTables
        public async Task<IActionResult> Index(
      string searchString,
      string sortOrder,
      string status,
      int? page)
        {
            ViewBag.CapacitySort = string.IsNullOrEmpty(sortOrder) ? "capacity_desc" : "";
            ViewBag.SearchString = searchString;
            ViewBag.Status = status;

            var tables = _context.RestaurantTables.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                tables = tables.Where(t =>
                    t.Status.Contains(searchString));
            }

            // Filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                tables = tables.Where(t => t.Status == status);
            }

            // Sorting
            switch (sortOrder)
            {
                case "capacity_desc":
                    tables = tables.OrderByDescending(t => t.Capacity);
                    break;

                default:
                    tables = tables.OrderBy(t => t.Capacity);
                    break;
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(await tables.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: RestaurantTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTable = await _context.RestaurantTables
                .FirstOrDefaultAsync(m => m.TableId == id);
            if (restaurantTable == null)
            {
                return NotFound();
            }
            ViewBag.StatusList = new List<string>
            {
                  "Available",
                  "Occupied"
            };

            return View(restaurantTable);

        }

        // GET: RestaurantTables/Create
        public IActionResult Create()
        {
            ViewBag.StatusList = new List<string>
             {
                "Available",
                "Reserved"
             };

            return View();
        }

        // POST: RestaurantTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TableId,Capacity,Status")] RestaurantTable restaurantTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurantTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurantTable);
        }

        // GET: RestaurantTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTable = await _context.RestaurantTables.FindAsync(id);
            if (restaurantTable == null)
            {
                return NotFound();
            }
            ViewBag.StatusList = new List<string>
            {
                 "Available",
                 "Reserved"
            };
            return View(restaurantTable);
        }

        // POST: RestaurantTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TableId,Capacity,Status")] RestaurantTable restaurantTable)
        {
            if (id != restaurantTable.TableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantTableExists(restaurantTable.TableId))
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
            return View(restaurantTable);
        }

        // GET: RestaurantTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTable = await _context.RestaurantTables
                .FirstOrDefaultAsync(m => m.TableId == id);
            if (restaurantTable == null)
            {
                return NotFound();
            }

            return View(restaurantTable);
        }

        // POST: RestaurantTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantTable = await _context.RestaurantTables.FindAsync(id);

            try
            {
                if (restaurantTable != null)
                {
                    _context.RestaurantTables.Remove(restaurantTable);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Table deleted successfully.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] =
                    "Cannot delete this table because it is linked to existing orders.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantTableExists(int id)
        {
            return _context.RestaurantTables.Any(e => e.TableId == id);
        }
    }
}
