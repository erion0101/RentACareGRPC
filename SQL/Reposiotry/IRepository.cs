using RentACareGRPC.SQL;

namespace RentACareGRPC.SQL
{
    public interface IRepository<T> where T : BaseViewModel
    {
        Task<T?> Get(int id, CancellationToken token);
        Task<Reservations> ReservationById(int id, CancellationToken token);
        IQueryable<T> GetAll();
        IQueryable<Role> GetRoleName();
        IQueryable<Role> GetRolesByCustomerId(int customerId);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task SaveAsync(CancellationToken token);
        IQueryable<Reservations> GetAllReservations();
        Task<Customers> FindCustomersByEmail(string email, CancellationToken cancellationToken);
    }
}
