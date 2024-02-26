namespace YennieStore.Areas.Admin.Models
{
    public class RevenueStatisticsViewModel
    {
        public int TotalRevenue { get; set; }
        public List<MonthlyRevenueViewModel> MonthlyRevenue { get; set; }
        public List<YearlyRevenueViewModel> YearlyRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewOrders { get; set; }
    }

    public class MonthlyRevenueViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Revenue { get; set; }
    }

    public class YearlyRevenueViewModel
    {
        public int Year { get; set; }
        public int Revenue { get; set; }
    }
}
