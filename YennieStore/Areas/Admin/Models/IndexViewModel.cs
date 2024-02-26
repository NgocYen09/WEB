namespace YennieStore.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public List<BrandProductCountViewModel> ProductCounts { get; set; }
        public int TotalRevenue { get; set; }
        public List<MonthlyRevenueViewModel> MonthlyRevenue { get; set; }
        public List<YearlyRevenueViewModel> YearlyRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewOrders { get; set; }
        public int TotalProductCount { get; set; }
    }
}
