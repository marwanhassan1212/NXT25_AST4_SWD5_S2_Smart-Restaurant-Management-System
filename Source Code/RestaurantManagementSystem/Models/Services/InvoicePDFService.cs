using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Services
{
    public class InvoicePdfService
    {
        public byte[] Generate(Order order)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Restaurant Management System")
                        .FontSize(22)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Invoice #: {order.OrderId}");
                        col.Item().Text($"Date: {order.OrderDate:dd/MM/yyyy HH:mm}");
                        col.Item().Text($"Customer: {order.Customer.FullName}");
                        col.Item().Text($"Cashier: {order.Employee.FullName}");
                        col.Item().Text($"Table: {order.Table.TableDisplay}");

                        col.Item().PaddingVertical(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Item").Bold();
                                header.Cell().Text("Qty").Bold();
                                header.Cell().Text("Price").Bold();
                                header.Cell().Text("Total").Bold();
                            });

                            foreach (var item in order.OrderDetails)
                            {
                                table.Cell().Text(item.Item.ItemName);
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text($"{item.UnitPrice:N2}");
                                table.Cell().Text($"{item.UnitPrice * item.Quantity:N2}");
                            }
                        });

                        col.Item().AlignRight().PaddingTop(20)
                            .Text($"Grand Total: {order.TotalAmount:N2} EGP")
                            .Bold()
                            .FontSize(18);
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Thank you for visiting us ❤️");
                });
            }).GeneratePdf();
        }
    }
}