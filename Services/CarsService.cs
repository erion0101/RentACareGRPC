using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentACareGRPC.Mapping;
using RentACareGRPC.SQL;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace RentACareGRPC.Services
{
    [Authorize(Policy = "Admin")]
    public class CarsService : CarsProtoService.CarsProtoServiceBase
    {
        private readonly IRepository<Cars> _repository;

        public CarsService(IRepository<Cars> repository)
        {
            this._repository = repository;
        }
        public override async Task<CreateCarsRespons> CreateCars(CreateCarsRequest request, ServerCallContext context)
        {
            if(request.Model is null || request.Year is null || request.Color is null || request.Brend is null || request.PriceForDay == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            var cars = new Cars
            {
                Brend = request.Brend,
                Model = request.Model,
                Color = request.Color,
                Year = request.Year,
                PriceForDay = (decimal)request.PriceForDay
            };
            await _repository.Add(cars);
            await _repository.SaveAsync(context.CancellationToken);

            return await Task.FromResult(new CreateCarsRespons
            {
                Id = cars.Id,
            });
        }
        public override async Task<ReadByIdCarsRespons> ReadByIdCars(ReadByIdCarsRequest request, ServerCallContext context)
        {
            if(request.Id <= 0)
                 throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

            var cars = await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == request.Id,context.CancellationToken);
            if(cars is not null)
            {
                return await Task.FromResult(new ReadByIdCarsRespons
                {
                    Brend = cars.Brend,
                    Model = cars.Model,
                    Year = cars.Year,
                    Color = cars.Color,
                    PriceForDay = (double)cars.PriceForDay
                });
            }

            throw new RpcException(new Status(StatusCode.InvalidArgument, $"No task with Id {request.Id}"));
        }
        public override async Task<ReadAllCarsRespons> ReadAllCars(ReadAllCarsRequest request, ServerCallContext context)
        {
            var respons = new ReadAllCarsRespons();
            var carsList = await _repository.GetAll().ToListAsync(context.CancellationToken);

            if (carsList is null)
                return null;
            foreach (var car in carsList)
            {
                respons.CarsProto.Add(new ReadByIdCarsRespons
                {
                    Brend = car.Brend,
                    Model = car.Model,
                    Year = car.Year,
                    Color = car.Color,
                    PriceForDay = (double)car.PriceForDay
                });
            }
            return await Task.FromResult(respons);
        }
    }
}
