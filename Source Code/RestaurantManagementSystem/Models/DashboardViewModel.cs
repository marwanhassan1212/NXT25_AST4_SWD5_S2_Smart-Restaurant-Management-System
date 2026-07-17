namespace RestaurantManagementSystem.Models
{
    public class DashboardViewModel
    {
        public int CustomersCount { get; set; }

        public int EmployeesCount { get; set; }

        public int OrdersCount { get; set; }

        public int MenuItemsCount { get; set; }

        public int CategoriesCount { get; set; }

        public int TablesCount { get; set; }

        public int RolesCount { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TodayRevenue { get; set; }

        public int TodayOrders { get; set; }

        public int AvailableTables { get; set; }

        public int ReservedTables { get; set; }

        public int PendingOrders { get; set; }

        public int PreparingOrders { get; set; }

        public int CompletedOrders { get; set; }

        public int CancelledOrders { get; set; }

        public List<string> Last7Days { get; set; } = new();

        public List<int> OrdersPerDay { get; set; } = new();

        public List<MenuItemsSales> TopSellingItems { get; set; } = new();

        public List<Order> RecentOrders { get; set; } = new();

        public decimal AverageOrderValue { get; set; }

        public string TopEmployeeName { get; set; } = "";

        public int TopEmployeeOrders { get; set; }
    }
}
