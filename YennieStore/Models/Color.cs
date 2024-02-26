namespace YennieStore.Models
{
    public partial class Color
    {
        public Color()
        {
            Products = new HashSet<Product>();
        }
        public int ?IdColor { get; set; }
        public string? NameColor { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
