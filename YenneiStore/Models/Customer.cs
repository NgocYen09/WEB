namespace YenneiStore.Models
{
    public partial class Customer
    { 
        public Customer() {
            Orders = new HashSet<Order>();        
        }
        public int CustomerId {  get; set; }
        public string FullName {  get; set; }
        public DateTime Birthday { get; set; }
        public string Address {  get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        public string Password {  get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
