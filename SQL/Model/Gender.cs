using System.ComponentModel.DataAnnotations.Schema;

namespace RentACareGRPC.SQL

{
    [Table("Gender")]
    public class Gender : BaseViewModel
    {
        [Column("GenderName")]
        public string GenderName { get; set; }

    }
}
