using System.ComponentModel.DataAnnotations.Schema;

namespace RentACareGRPC.SQL
{
    [Table("Customers")]
    public class Customers : BaseViewModel
    {
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("NrLetenjoftimit")]
        public int NrLetenjoftimit { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("GenderId")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        [Column("AdresaId")]
        public int AdresaId { get; set; }
        public Adress Adress { get; set; }
        [Column("RoleId")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public static implicit operator Customers(Task<Customers?> v)
        {
            throw new NotImplementedException();
        }
    }
}
