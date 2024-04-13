using Azure;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentACareGRPC.Mapping;
using RentACareGRPC.SQL;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACareGRPC.Services
{
    [Authorize(Policy = "PermissionMemeberCEO")]
    public class reservationService : ReservationProto.ReservationProtoBase
    {
        private readonly IRepository<Reservations> _repository;
        public reservationService(IRepository<Reservations> repository)
        {
            _repository = repository;
        }
        public override async Task<CreateReservationRespons> CreateReservation(CreateReservationRequest request, ServerCallContext context)
        {
            if(request.StartDate ==  null || request.EndDate == null || request.TotalPrice == null || request.CarId == null || request.CustomerId ==  null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));
            var reservation = new Reservations()
            {
                StartDate = request.StartDate.ToDateTime(),
                EndDate = request.EndDate.ToDateTime(),
                TotalPrice = (decimal)request.TotalPrice,
                CarId = request.CarId,
                Cars = CarsMapping.ToPROTO(request.Cars),
                CustomerId = request.CustomerId,
                Customers = CustomersMapping.ToModel(request.Customers),
            };
            await _repository.Add(reservation);
            await _repository.SaveAsync(context.CancellationToken);

            return await Task.FromResult(new CreateReservationRespons
            {
                Id = reservation.Id,
            });
        }
        public override async Task<ReadReservationByIdRespons> ReadReservationById(ReadReservationByIdRequest request, ServerCallContext context)
        {
            if(request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Please input a valid value"));

            if (request.Id == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Please input one value valid"));
            var reservationId = await _repository.ReservationById(request.Id, context.CancellationToken);
            if (reservationId is not null)
            {
                return await Task.FromResult(new ReadReservationByIdRespons { 

                    StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(reservationId.StartDate.ToUniversalTime()),
                    EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(reservationId.EndDate.ToUniversalTime()),
                    TotalPrice = (double)reservationId.TotalPrice,
                    CarId = reservationId.CarId,
                    Cars = CarsMapping.ToModel(reservationId.Cars),
                    CustomerId = reservationId.CustomerId,
                    Customers = CustomersMapping.ToDTO(reservationId.Customers),

                });
            }
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"No task with Id {request.Id}"));

        }
        public override async Task<ReadAllReservationResponse> ReadAllReservation(ReadAllReservationRequest request, ServerCallContext context)
        {
            var respons = new ReadAllReservationResponse();
            var reservationList = _repository.GetAllReservations();

            if (reservationList == null)
                return null;
            foreach (var reservation in reservationList)
            {
                respons.ReservatIon.Add(new ReadReservationByIdRespons
                {
                    StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(reservation.StartDate.ToUniversalTime()),
                    EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(reservation.EndDate.ToUniversalTime()),
                    TotalPrice = (double)reservation.TotalPrice,
                    CarId = reservation.CarId,
                    Cars = CarsMapping.ToModel(reservation.Cars),
                    CustomerId = reservation.CustomerId,
                    Customers = CustomersMapping.ToDTO(reservation.Customers),

                });
            }

            return await Task.FromResult(respons);
        }
    }
}
