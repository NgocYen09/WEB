using System.ComponentModel.DataAnnotations.Schema;

namespace YennieStore.Models
{
    public partial class FeedBack
    {
        public int? FeedBackId { get; set; }
        public string? Email { get; set; }
        public int? CustomerId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Status { get; set; }
        [NotMapped]
        public string ?CustomerName { get; set; }
        public virtual Customer ?Customer { get; set; }
    }
}
