using RentACareGRPC.SQL;

namespace RentACareGRPC.Mapping
{
    public static class CustomersMapping
    {
        public static Customers ToModel(CustomersPROTOo customers)
        {
            if (customers == null)
            {
                return null;
            }
            return new Customers
            {
                Id = customers.Id,
                FirstName = customers.FirstName,
                LastName = customers.LastName,
                Email = customers.Email,
                Password = customers.Password,
                Phone = customers.Phone,
                AdresaId = customers.AdressId,
                Adress = AdresaMapping.ToModelo(customers.AdressPROTO),
                GenderId = customers.GenderId,
                Gender = GenderMapping.ToModelo(customers.GenderPROTO),
                RoleId = customers.RoleId,
                Role = RoleMapping.ToModelo(customers.RolePROTO),
            };
        }
        public static CustomersPROTOo ToDTO(Customers customers)
        {
            if (customers == null)
            {
                return null;
            }
            return new CustomersPROTOo
            {
                Id = customers.Id,
                FirstName = customers.FirstName,
                LastName = customers.LastName,
                Email = customers.Email,
                Password = customers.Password,
                Phone = customers.Phone,
                AdressId = customers.AdresaId,
                AdressPROTO = AdresaMapping.ToDTOo(customers.Adress),
                GenderId = customers.GenderId,
                GenderPROTO = GenderMapping.ToDTOo(customers.Gender),
                RoleId = customers.RoleId,
                RolePROTO = RoleMapping.ToDTOo(customers.Role),
            };
        }
        public static CustomersPROTOo ToDTOForReservation(Customers customers) => new()
        {
            //Id = customers.Id,
            FirstName = customers.FirstName,
            LastName = customers.LastName,
            Email = customers.Email,
            Password = customers.Password,
            Phone = customers.Phone,
            AdressId = customers.AdresaId,
            AdressPROTO = AdresaMapping.ToDTOo(customers.Adress),
            GenderId = customers.GenderId,
            GenderPROTO = GenderMapping.ToDTOo(customers.Gender),
            RoleId = customers.RoleId,
            RolePROTO = RoleMapping.ToDTOo(customers.Role),
        };


        public static IEnumerable<CustomersPROTOo> ToDTOs(this IEnumerable<Customers> cars) =>
            cars.Select(s => ToDTO(s));
    }
}
