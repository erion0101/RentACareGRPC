using RentACareGRPC.SQL;

namespace RentACareGRPC.Mapping
{
    public static class PermissionMapping
    {
        public static Permission ToModel(PermissionPROTO permission)
        {
            if (permission == null)
            {
                return null;
            }
            return new Permission
            {
                Id = permission.Id,
                PermissionName = permission.PermissionName
            };
        }
        public static Permission ToModelo(PermissionPROTOo permission)
        {
            if (permission == null)
            {
                return null;
            }
            return new Permission
            {
                Id = permission.Id,
                PermissionName = permission.PermissionName
            };
        }

        public static PermissionPROTO ToDTO(Permission permission)
        {
            if (permission == null)
            {
                return null; // Or handle the null case appropriately
            }
            return new PermissionPROTO
            {
                Id = permission.Id,
                PermissionName = permission.PermissionName
            };
        }
        public static PermissionPROTOo ToDTOo(Permission permission)
        {
            if (permission == null)
            {
                return null; // Or handle the null case appropriately
            }
            return new PermissionPROTOo
            {
                Id = permission.Id,
                PermissionName = permission.PermissionName
            };
        }
        public static IEnumerable<PermissionPROTO> ToDTOs(this IEnumerable<Permission> permission) =>
               permission.Select(s => ToDTO(s));
    }
}
