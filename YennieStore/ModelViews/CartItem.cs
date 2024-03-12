using YennieStore.Models;

namespace YennieStore.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.Discount.Value;
    }
}
