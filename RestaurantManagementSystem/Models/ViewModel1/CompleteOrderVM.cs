namespace RestaurantManagementSystem.ViewModels
{
    public class CompleteOrderVM
    {
        public int CustomerId { get; set; }

        public int TableId { get; set; }

        public List<OrderItemVM> Items { get; set; } = new();
    }

    public class OrderItemVM
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }
    }
}