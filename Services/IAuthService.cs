using RentACareGRPC.SQL;

namespace RentACareGRPC.Services
{
    public interface IAuthService
    {
        Task<Customers?> FindCustomersByEmail(string email, CancellationToken token);
    }
}
