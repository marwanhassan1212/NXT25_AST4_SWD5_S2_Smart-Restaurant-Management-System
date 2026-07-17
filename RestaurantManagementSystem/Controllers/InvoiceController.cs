using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Services;


namespace RestaurantManagementSystem.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {

        private readonly InvoicePdfService _invoicePdfService;

        public InvoiceController(
            RestaurantDbContext context,
            InvoicePdfService invoicePdfService)
        {
            _context = context;
            _invoicePdfService = invoicePdfService;
        }

        private readonly RestaurantDbContext _context;

      

        public async Task<IActionResult> Index(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Table)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.Table)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            var pdf = _invoicePdfService.Generate(order);

            return File(
                pdf,
                "application/pdf",
                $"Invoice-{order.OrderId}.pdf");
        }
    }
}