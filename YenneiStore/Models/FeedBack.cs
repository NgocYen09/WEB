using System.ComponentModel.DataAnnotations.Schema;

namespace YenneiStore.Models
{ //góp ý
    public partial class FeedBack
    {
        public int FeedBackId {  get; set; }
        public string Email {  get; set; }
        public int CustomerId {  get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public string CustomerName {  get; set; }
        public virtual Customer Customers { get; set; }
    }
}
