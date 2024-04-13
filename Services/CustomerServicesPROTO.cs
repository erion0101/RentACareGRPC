using Azure;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentACareGRPC.Mapping;
using RentACareGRPC.SQL;
using System.Collections;
using System.Numerics;

namespace RentACareGRPC.Services
{
    [Authorize(Policy = "PermissionAdmin")]
    public class CustomerServicesPROTO : CustomersProto.CustomersProtoBase
    {
        private readonly IRepository<Customers> _repository;
        public CustomerServicesPROTO(IRepository<Customers> repository)
        {
            _repository = repository;
        }
        public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            if (request.FirstName == null || request.LastName == null || request.Email == null
                || request.Password == null || request.NrLetenjoftimit == null
                || request.Phone == null || request.AdressId == null
                || request.GenderId == null || request.RoleId == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));
            var item = new Customers()
            {
                
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                NrLetenjoftimit = request.NrLetenjoftimit,
                Phone = request.Phone,
                AdresaId = request.AdressId,
                Adress = AdresaMapping.ToModel(request.AdressPROTO),
                GenderId = request.GenderId,
                Gender = GenderMapping.ToModel(request.GenderPROTO),
                RoleId = request.RoleId,
                Role = RoleMapping.ToModel(request.RolePROTO)
            };
            await _repository.Add(item);
            await _repository.SaveAsync(context.CancellationToken);
            return await Task.FromResult(new CreateCustomerResponse
            {
                Id = item.Id,
            });
        }
        public override async Task<GetCustomerByIdResponse> GetCustomerById(GetCustomerByIdRequest request, ServerCallContext context)
        {
            if (request.CustomersId <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "resouce index must be greater than 0"));

            var customerId = await _repository.GetAll().FirstOrDefaultAsync(e => e.Id == request.CustomersId);
            if(customerId != null)
            {
                return await Task.FromResult(new GetCustomerByIdResponse
                {
                    Id = customerId.Id,
                    FirstName = customerId.FirstName,
                    LastName = customerId.LastName,
                    Email = customerId.Email,
                    Password = customerId.Password,
                    NrLetenjoftimit = customerId.NrLetenjoftimit,
                    Phone = customerId.Phone,
                    AdressPROTO = AdresaMapping.ToDTO(customerId.Adress),
                    GenderId = customerId.GenderId,
                    GenderPROTO = GenderMapping.ToDTO(customerId.Gender),
                    RoleId = customerId.RoleId,
                    RolePROTO = RoleMapping.ToDTO(customerId.Role)
                });
            }
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"No task with Id {request.CustomersId}"));
        }
        public override async Task<GetAllCustomerRespons> GetAllCustomers(GetAllCustomerRequest request, ServerCallContext context)
        {
            var respons = new GetAllCustomerRespons();
            var customertolist = await _repository.GetAll().ToListAsync(context.CancellationToken);
            if (customertolist == null)
                return null;
            foreach ( var customer in customertolist )
            {
                respons.CustuMers.Add(new GetCustomerByIdResponse
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Password = customer.Password,
                    NrLetenjoftimit = customer.NrLetenjoftimit,
                    Phone = customer.Phone,
                    AdressPROTO = AdresaMapping.ToDTO(customer.Adress),
                    GenderId = customer.GenderId,
                    GenderPROTO = GenderMapping.ToDTO(customer.Gender),
                    RoleId = customer.RoleId,
                    RolePROTO = RoleMapping.ToDTO(customer.Role)
                });
            }

            return await Task.FromResult(respons);
        }
        public override async Task<CustomersForEmailRespons> FindCustomersByEmail(FindCustomersByEmailRequest request, ServerCallContext context)
        {
            if(request == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));
            var respons = new CustomersForEmailRespons();
            
            var email =  await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == request.Email, context.CancellationToken);
            if (email != null)
            {
                return await Task.FromResult(new CustomersForEmailRespons
                {
                    Id = email.Id,
                    FirstName = email.FirstName,
                    LastName = email.LastName,
                    Email = email.Email,
                    Password = email.Password,
                    NrLetenjoftimit = email.NrLetenjoftimit,
                    Phone = email.Phone,
                    AdressPROTO = AdresaMapping.ToDTO(email.Adress),
                    GenderId = email.GenderId,
                    GenderPROTO = GenderMapping.ToDTO(email.Gender),
                    RoleId = email.RoleId,
                    RolePROTO = RoleMapping.ToDTO(email.Role)
                });
            }
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"No task with Id {email.Id}"));

        }
        public override async Task<CustomersByRoleIdRespons> GetRoleNameById(GetCustomerIdRequest request, ServerCallContext context)
        {
            if(request.Id == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id can not be null"));
            if(request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id can not be less 0"));
            var role = await _repository.GetRoleName().FirstOrDefaultAsync(x => x.Id == request.Id);
            if(role != null)
            {
                return await Task.FromResult(new CustomersByRoleIdRespons
                {
                    Id = request.Id,
                    Name = role.Name,
                    PermissionId = role.PermissionId,
                    PermissionPROTO = PermissionMapping.ToDTO(role.Permission)
                });
            }
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"No role with Id {role.Id}"));
        }
        public override async Task<DeleteCusomersRespons> DeltetCustomers(DeleteCustomersRequest request, ServerCallContext context)
        {
            if (request.Id == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "can not input a null value"));
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id can not be less 0"));

            var findId = await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == request.Id,context.CancellationToken);

            if (findId == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"No Task with Id {request.Id}"));

            await _repository.Delete(findId.Id);
            await _repository.SaveAsync(context.CancellationToken);

            return await Task.FromResult(new DeleteCusomersRespons
            {
                Id = findId.Id,
            });

        }
        public override async Task<UpdateCustomersRespons> UpdateCustomers(UpdateCustomersRequest request, ServerCallContext context)
        {
            if (request.FirstName == null || request.LastName == null || request.Email == null
               || request.Password == null || request.NrLetenjoftimit == null
               || request.Phone == null || request.AdressId == null
               || request.GenderId == null || request.RoleId == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must suppply a valid object"));

            Customers customersUpdate = await _repository.Get(request.Id,context.CancellationToken);

            if (customersUpdate == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"No Task with Id {request.Id}"));
            
            customersUpdate.FirstName = request.FirstName;
            customersUpdate.LastName = request.LastName;
            customersUpdate.Email = request.Email;
            customersUpdate.Password = request.Password;
            customersUpdate.NrLetenjoftimit = request.NrLetenjoftimit;
            customersUpdate.Phone = request.Phone;
            customersUpdate.AdresaId = request.AdressId;
            customersUpdate.GenderId = request.GenderId;
            customersUpdate.RoleId = request.RoleId;

            await _repository.SaveAsync(context.CancellationToken);
            return await Task.FromResult(new UpdateCustomersRespons
            {
                Id = customersUpdate.Id,
            });

        }

    }
}
