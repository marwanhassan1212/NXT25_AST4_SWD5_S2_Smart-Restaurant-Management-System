using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using System.Diagnostics;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RestaurantDbContext _context;
        public HomeController(
         ILogger<HomeController> logger,
        RestaurantDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboard = new DashboardViewModel
            {
                CustomersCount = _context.Customers.Count(),

                EmployeesCount = _context.Employees.Count(),

                OrdersCount = _context.Orders.Count(),

                MenuItemsCount = _context.MenuItems.Count(),

                CategoriesCount = _context.Categories.Count(),

                TablesCount = _context.RestaurantTables.Count(),

                RolesCount = _context.Roles.Count(),

                TotalRevenue = _context.Orders.Sum(o => o.TotalAmount),
                TodayRevenue = _context.Orders
                .Where(o => o.OrderDate.Date == DateTime.Today)
                .Sum(o => (decimal?)o.TotalAmount) ?? 0,

                            TodayOrders = _context.Orders
                .Count(o => o.OrderDate.Date == DateTime.Today),

                            AvailableTables = _context.RestaurantTables
                .Count(t => t.Status == "Available"),

                            ReservedTables = _context.RestaurantTables
                .Count(t => t.Status == "Reserved"),

                PendingOrders = _context.Orders.Count(o => o.OrderStatus == "Pending"),

                PreparingOrders = _context.Orders.Count(o => o.OrderStatus == "Preparing"),

                CompletedOrders = _context.Orders.Count(o => o.OrderStatus == "Completed"),

                CancelledOrders = _context.Orders.Count(o => o.OrderStatus == "Cancelled"),

                AverageOrderValue = _context.Orders.Any()
                ? _context.Orders.Average(o => o.TotalAmount)
                : 0,

                RecentOrders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Table)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList(),

                TopSellingItems = _context.OrderDetails
                .Include(x => x.Item)
                .GroupBy(x => new
                {
                    x.Item.ItemName
                })
                .Select(g => new MenuItemsSales
                {
                    ItemName = g.Key.ItemName,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList(),
            };
            var topEmployee = _context.Orders
            .Include(o => o.Employee)
            .AsEnumerable()
            .GroupBy(o => o.Employee.FullName)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

            if (topEmployee != null)
            {
                dashboard.TopEmployeeName = topEmployee.Key;
                dashboard.TopEmployeeOrders = topEmployee.Count();
            }

            var last7Days = Enumerable.Range(0, 7)
            .Select(i => DateTime.Today.AddDays(-6 + i))
            .ToList();

                    dashboard.Last7Days = last7Days
                        .Select(d => d.ToString("dd MMM"))
                        .ToList();

                    dashboard.OrdersPerDay = last7Days
                        .Select(d => _context.Orders.Count(o => o.OrderDate.Date == d.Date))
                        .ToList();



            return View(dashboard);
         
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
