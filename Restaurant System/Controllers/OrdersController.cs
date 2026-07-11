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
    public class OrdersController : Controller
    {
        private readonly RestaurantDbContext _context;

        public OrdersController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var restaurantDbContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.Table);
            return View(await restaurantDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(
            _context.Customers,
            "CustomerId",
            "FullName");

            ViewData["EmployeeId"] = new SelectList(
            _context.Employees,
            "EmployeeId",
            "FullName");

            ViewData["TableId"] = new SelectList(
            _context.RestaurantTables,
            "TableId",
            "TableDisplay");
            ViewBag.OrderStatusList = new List<string>
            {
            "Pending",
            "Preparing",
            "Completed",
            "Cancelled"
            };
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,OrderStatus,TotalAmount,EmployeeId,CustomerId,TableId")] Order order)
        {
            ModelState.Remove(nameof(Order.Customer));
            ModelState.Remove(nameof(Order.Employee));
            ModelState.Remove(nameof(Order.Table));
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(
            _context.Customers,
            "CustomerId",
            "FullName",
            order.CustomerId);

            ViewData["EmployeeId"] = new SelectList(
            _context.Employees,
            "EmployeeId",
            "FullName",
            order.EmployeeId);

            ViewData["TableId"] = new SelectList(
            _context.RestaurantTables,
            "TableId",
            "TableDisplay",
            order.TableId);
            ViewBag.OrderStatusList = new List<string>
            {
                "Pending",
                "Preparing",
                "Completed",
                "Cancelled"
            };

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(
                 _context.Customers,
                "CustomerId",
                "FullName",
                order.CustomerId);

            ViewData["EmployeeId"] = new SelectList(
            _context.Employees,
            "EmployeeId",
            "FullName",
            order.EmployeeId);

            ViewData["TableId"] = new SelectList(
            _context.RestaurantTables,
             "TableId",
            "TableDisplay",
            order.TableId);
            ViewBag.OrderStatusList = new List<string>
            {
                "Pending",
                "Preparing",
                "Completed",
                "Cancelled"
            };
            return View(order);

        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,OrderStatus,TotalAmount,EmployeeId,CustomerId,TableId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(Order.Customer));
            ModelState.Remove(nameof(Order.Employee));
            ModelState.Remove(nameof(Order.Table));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(
            _context.Customers,
            "CustomerId",
            "FullName",
            order.CustomerId);

            ViewData["EmployeeId"] = new SelectList(
            _context.Employees,
            "EmployeeId",
            "FullName",
    order.EmployeeId);

            ViewData["TableId"] = new SelectList(
            _context.RestaurantTables,
             "TableId",
            "TableDisplay",
            order.TableId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
