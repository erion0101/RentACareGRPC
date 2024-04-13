using RentACareGRPC.SQL;

namespace RentACareGRPC.Mapping
{
    public static class CarsMapping
    {
        public static Cars ToPROTO(CarsPROTOo adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new Cars
            { 
                Brend = adress.Brend,
                Model = adress.Model,
                Year = adress.Year,
                Color = adress.Color,
                PriceForDay = (decimal)adress.PriceForDay
            };
        }
        public static CarsPROTOo ToModel(Cars adress)
        {
            if (adress == null)
            {
                return null;
            }
            return new CarsPROTOo
            {
                Brend = adress.Brend,
                Model = adress.Model,
                Year = adress.Year,
                Color = adress.Color,
                PriceForDay = (double)adress.PriceForDay
            };
        }
        public static IEnumerable<CarsPROTOo> ToModels(this IEnumerable<Cars> adress) =>
               adress.Select(s => ToModel(s));
    }
}
