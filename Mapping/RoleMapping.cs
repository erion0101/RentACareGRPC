using RentACareGRPC.SQL;

namespace RentACareGRPC.Mapping
{
    public static class RoleMapping
    {
        public static Role ToModel(RolePROTO adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new Role
            {
                Name = adress.Name,
                PermissionId = adress.PermissionId,
                Permission = PermissionMapping.ToModel(adress.PermissionPROTO)
            };
        }
        public static Role ToModelo(RolePROTOo adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new Role
            {
                Name = adress.Name,
                PermissionId = adress.PermissionId,
                Permission = PermissionMapping.ToModelo(adress.PermissionPROTO)
            };
        }
        public static RolePROTO ToDTO(Role adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new RolePROTO
            {
                Name = adress.Name,
                PermissionId = adress.PermissionId,
                PermissionPROTO = PermissionMapping.ToDTO(adress.Permission)
            };

        }
        public static RolePROTOo ToDTOo(Role adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new RolePROTOo
            {
                Name = adress.Name,
                PermissionId = adress.PermissionId,
                PermissionPROTO = PermissionMapping.ToDTOo(adress.Permission)
            };

        }
        public static IEnumerable<RolePROTO> ToDTOs(this IEnumerable<Role> adress) =>
                  adress.Select(s => ToDTO(s));
    }
}
