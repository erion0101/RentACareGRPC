using Microsoft.EntityFrameworkCore;
using RentACareGRPC.SQL;

namespace RentACareGRPC.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<Customers> _repository;

        public AuthService(IRepository<Customers> repository) 
        {
            _repository = repository;
        }

        public async Task<Customers?> FindCustomersByEmail(string email, CancellationToken token)
        {
            return await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == email, token);
        }

    }
}
