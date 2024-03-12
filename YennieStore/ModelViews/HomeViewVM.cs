using YennieStore.Models;

namespace YennieStore.ModelViews
{
    public class HomeViewVM
    {
        public List<TinDang> TinTucs { get; set; }
        public List<ProductHomeVM> Products { get; set; }
        public List<Product> BestSellerProducts { get; set; }
        public QuangCao quangcao { get; set; }
    }
}
