namespace YenneiStore.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }
        public int IdBrand { get; set; }
        public string NameBrand { get; set; }
        public string Thumb {  get; set; }
        public bool Published {  get; set; }
        public virtual ICollection<Product> Products { get; set; }
       
    }
}
