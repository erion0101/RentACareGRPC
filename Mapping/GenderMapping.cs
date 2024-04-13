using RentACareGRPC.SQL;

namespace RentACareGRPC.Mapping
{
    public static class GenderMapping
    {
        public static Gender ToModel(GenderPROTO gender)
        {
            if (gender == null)
            {
                return null;
            }
            return new Gender
            {
                GenderName = gender.GenderName
            };
        }
        public static Gender ToModelo(GenderPROTOo gender)
        {
            if (gender == null)
            {
                return null;
            }
            return new Gender
            {
                GenderName = gender.GenderName
            };
        }
        public static GenderPROTO ToDTO(Gender gender)
        {
            if (gender == null)
            {
                return null;
            }
            return new GenderPROTO
            {
                GenderName = gender.GenderName
            };
        }
        public static GenderPROTOo ToDTOo(Gender gender)
        {
            if (gender == null)
            {
                return null;
            }
            return new GenderPROTOo
            {
                GenderName = gender.GenderName
            };
        }
        public static IEnumerable<GenderPROTO> ToDTOs(this IEnumerable<Gender> gender) =>
               gender.Select(s => ToDTO(s));
    }
}
