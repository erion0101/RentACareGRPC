using System.ComponentModel.DataAnnotations.Schema;

namespace RentACareGRPC.SQL
{
    [Table("Reservations")]
    public class Reservations : BaseViewModel
    {
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("TotalPrice")]
        public decimal TotalPrice { get; set; }
        [Column("CarId")]
        public int CarId { get; set; }
        public Cars? Cars { get; set; }
        [Column("CustomerId")]
        public int CustomerId { get; set; }
        public Customers? Customers { get; set; }
    }
}
