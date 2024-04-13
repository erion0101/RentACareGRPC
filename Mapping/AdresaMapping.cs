using RentACareGRPC.SQL;

namespace RentACareGRPC.Mapping
{
    public static class AdresaMapping
    {
        public static Adress ToModel(AdressPROTO adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new Adress
            {
                City = adress.City,
                StreetAddress = adress.StreetAddress,
                ZipCode = adress.ZipCode,
            };
        }
        public static Adress ToModelo(AdressPROTOo adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new Adress
            {
                City = adress.City,
                StreetAddress = adress.StreetAddress,
                ZipCode = adress.ZipCode,
            };
        }
        public static AdressPROTO ToDTO(Adress adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new AdressPROTO
            {
                City = adress.City,
                StreetAddress = adress.StreetAddress,
                ZipCode = adress.ZipCode,
            };
        }
        public static AdressPROTOo ToDTOo(Adress adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new AdressPROTOo
            {
                City = adress.City,
                StreetAddress = adress.StreetAddress,
                ZipCode = adress.ZipCode,
            };
        }
        public static IEnumerable<AdressPROTO> ToDTOs(this IEnumerable<Adress> adress) =>
               adress.Select(s => ToDTO(s));
    }
}
