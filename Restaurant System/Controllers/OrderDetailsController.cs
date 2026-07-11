using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Restaurant_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_System.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly RestaurantDbContext _context;

        public OrderDetailsController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var restaurantDbContext = _context.OrderDetails.Include(o => o.Item).Include(o => o.Order);
            return View(await restaurantDbContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? orderId, int? itemId)
        {
            if (orderId == null || itemId == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Item)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(o =>
                    o.OrderId == orderId &&
                    o.ItemId == itemId);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(
            _context.MenuItems,
            "ItemId",
            "ItemName");
            ViewData["OrderId"] = new SelectList(
            _context.Orders,
            "OrderId",
            "OrderDisplay");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ItemId,Quantity,UnitPrice")] OrderDetail orderDetail)
        {
            ModelState.Remove(nameof(OrderDetail.Order));
            ModelState.Remove(nameof(OrderDetail.Item));
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(
             _context.MenuItems,
             "ItemId",
             "ItemName",
             orderDetail.ItemId);

            ViewData["OrderId"] = new SelectList(
               _context.Orders,
               "OrderId",
               "OrderDisplay",
               orderDetail.OrderId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? orderId, int? itemId)
        {
            if (orderId == null || itemId == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
            .Include(o => o.Order)
            .Include(o => o.Item)
            .FirstOrDefaultAsync(o =>
                o.OrderId == orderId &&
                o.ItemId == itemId);

            if (orderDetail == null)
            {
                return NotFound();
            }

            ViewData["ItemId"] = new SelectList(
                _context.MenuItems,
                "ItemId",
                "ItemName",
                orderDetail.ItemId);

            ViewData["OrderId"] = new SelectList(
                _context.Orders,
                "OrderId",
                "OrderDisplay",
                orderDetail.OrderId);

            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
        int orderId,
        int itemId,
        [Bind("OrderId,ItemId,Quantity,UnitPrice")] OrderDetail orderDetail)
        {
            ModelState.Remove(nameof(OrderDetail.Order));
            ModelState.Remove(nameof(OrderDetail.Item));
            if (orderId != orderDetail.OrderId ||
                itemId != orderDetail.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderId, orderDetail.ItemId))
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
            ViewData["ItemId"] = new SelectList(
                _context.MenuItems,
                "ItemId",
                "ItemName",
                orderDetail.ItemId);

            ViewData["OrderId"] = new SelectList(
                _context.Orders,
                "OrderId",
                "OrderDisplay",
                orderDetail.OrderId);
              return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? orderId, int? itemId)
        {
            if (orderId == null || itemId == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
              .Include(o => o.Order)
              .Include(o => o.Item)
              .FirstOrDefaultAsync(o =>
                  o.OrderId == orderId &&
                  o.ItemId == itemId);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int orderId, int itemId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderId, itemId);

            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int orderId, int itemId)
        {
            return _context.OrderDetails.Any(e =>
                e.OrderId == orderId &&
                e.ItemId == itemId);
        }
    }
}
