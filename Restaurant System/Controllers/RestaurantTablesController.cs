using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_System.Models;

namespace Restaurant_System.Controllers
{
    public class RestaurantTablesController : Controller
    {
        private readonly RestaurantDbContext _context;

        public RestaurantTablesController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: RestaurantTables
        public async Task<IActionResult> Index()
        {
            return View(await _context.RestaurantTables.ToListAsync());
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
            if (restaurantTable != null)
            {
                _context.RestaurantTables.Remove(restaurantTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantTableExists(int id)
        {
            return _context.RestaurantTables.Any(e => e.TableId == id);
        }
    }
}
