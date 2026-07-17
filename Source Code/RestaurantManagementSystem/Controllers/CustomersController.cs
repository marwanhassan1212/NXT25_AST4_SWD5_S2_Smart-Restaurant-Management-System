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
    [Authorize(Roles = "Admin,Manager,Cashier,Waiter")]
    public class CustomersController : Controller
    {
      
        
            private readonly RestaurantDbContext _context;

            public CustomersController(RestaurantDbContext context)
            {
                _context = context;
            }

        // GET: Customers
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? page)
        {
            ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SearchString = searchString;

            var customers = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                customers = customers.Where(c =>
                    c.FullName.Contains(searchString) ||
                    c.PhoneNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.FullName);
                    break;

                default:
                    customers = customers.OrderBy(c => c.FullName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(await customers.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerId == id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }

            // GET: Customers/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Customers/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("CustomerId,FullName,PhoneNumber")] Customer customer)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(customer);
            }

            // GET: Customers/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }

            // POST: Customers/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,PhoneNumber")] Customer customer)
            {
                if (id != customer.CustomerId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CustomerExists(customer.CustomerId))
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
                return View(customer);
            }

            // GET: Customers/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerId == id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            try
            {
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Customer deleted successfully.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] =
                    "Cannot delete this customer because they have related orders.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
            {
                return _context.Customers.Any(e => e.CustomerId == id);
            }
       
    }
    }
