using Microsoft.Extensions.Primitives;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace YenneiStore.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId {  get; set; }
        public int ProductId {  get; set; }
        public string Email {  get; set; }
        public string Content {  get; set; }
        public DateTime CreatedDate {  get; set; }
        public string Status {  get; set; }
        public int Rate { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public String ProductName { get; set; }
        public virtual Customer Customer { get; set; }  
        public virtual Product Product { get; set; }    
    }
}
