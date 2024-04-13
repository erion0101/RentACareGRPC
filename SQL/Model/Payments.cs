using System.ComponentModel.DataAnnotations.Schema;

namespace RentACareGRPC.SQL
{
    [Table("Payments")]
    public class Payments : BaseViewModel
    {
        [Column("PaymentData")]
        public DateTime PaymentData { get; set; }
        [Column("Amount")]
        public decimal Amount { get; set; }
        [Column("ReservationId")]
        public int ReservationId { get; set; }
        public Reservations? Reservations { get; set; }

    }
}
