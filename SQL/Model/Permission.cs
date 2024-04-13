using System.ComponentModel.DataAnnotations.Schema;

namespace RentACareGRPC.SQL
{
    [Table("Permission")]
    public class Permission : BaseViewModel
    {
        [Column("PermissionName")]
        public string PermissionName { get; set; }
    }
}
