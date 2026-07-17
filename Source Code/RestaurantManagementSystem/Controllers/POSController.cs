using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.ViewModels;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize]
    public class POSController : Controller
    {
        private readonly RestaurantDbContext _context;

        public POSController(RestaurantDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var menuItems = _context.MenuItems
                .Include(m => m.Category)
                .Where(m => m.AvailabilityStatus)
                .ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Tables = _context.RestaurantTables
                .Where(t => t.Status == "Available")
                .ToList();

            return View(menuItems);
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrder([FromBody] CompleteOrderVM model)
        {
            if (model == null || model.Items.Count == 0)
            {
                return BadRequest("Order is empty.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int employeeId = int.Parse(
                    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

                decimal total = 0;

                foreach (var item in model.Items)
                {
                    var menuItem = await _context.MenuItems
                        .FirstOrDefaultAsync(x => x.ItemId == item.ItemId);

                    if (menuItem == null)
                        continue;

                    total += menuItem.Price * item.Quantity;
                }

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending",
                    TotalAmount = total,
                    EmployeeId = employeeId,
                    CustomerId = model.CustomerId,
                    TableId = model.TableId
                };

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                foreach (var item in model.Items)
                {
                    var menuItem = await _context.MenuItems
                        .FirstOrDefaultAsync(x => x.ItemId == item.ItemId);

                    if (menuItem == null)
                        continue;

                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitPrice = menuItem.Price
                    });
                }

                var table = await _context.RestaurantTables
                    .FirstOrDefaultAsync(t => t.TableId == model.TableId);

                if (table != null)
                {
                    table.Status = "Occupied";
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    success = true,
                    orderId = order.OrderId
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(ex.Message);
            }
        }
    }
}