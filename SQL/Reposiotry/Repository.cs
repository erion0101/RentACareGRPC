using Microsoft.EntityFrameworkCore;
using RentACareGRPC.SQL;
using RentACareGRPC.SQL;

namespace RentACareGRPC.SQL
{
    public class Repository<T> : IRepository<T> where T : BaseViewModel
    {
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(T entity)
        {
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var result = await _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _dbContext.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<T?> Get(int id, CancellationToken token) =>
           await _dbContext.Set<T>().FirstOrDefaultAsync(d => d.Id == id, token);
        public IQueryable<T> GetAll() =>
             _dbContext.Set<T>().AsQueryable();

        public IQueryable<Customers> GetAllCustomers() =>
            _dbContext.Set<Customers>().AsQueryable()
                                       .Include(i => i.Gender)
                                       .Include(i => i.Adress);

        public IQueryable<Reservations> GetAllReservations() =>
        _dbContext.Set<Reservations>().AsQueryable()
            .Include(i => i.Cars)
            .Include(e => e.Customers)
                .ThenInclude(c => c.Adress)
            .Include(e => e.Customers)
                .ThenInclude(c => c.Gender)
            .Include(e => e.Customers)
                .ThenInclude(c => c.Role).ThenInclude(p => p.Permission);

        public IQueryable<Role> GetRolesByCustomerId(int customerId)
        {
            return _dbContext.Set<Customers>()
            .Where(c => c.Id == customerId)
            .Select(c => c.Role)
            .Include(r => r.Permission);
        }

        public IQueryable<Role> GetRoleName()
        {
            return _dbContext.Customers
              .Select(c => new Role
              {
                  Id = c.RoleId,
                  Name = c.Role.Name,
                  Permission = c.Role.Permission != null ? c.Role.Permission : null
              });
            //  return _dbContext.Set<Customers>().AsQueryable().Include(i => i.Role)
            //.Include(r => r.Role.Permission);
        }
        //.Where(c => c.PermissionName != null || c.PermissionName == null)
        //.Select(c => c.RoleName);


        public async Task SaveAsync(CancellationToken token) =>
            await _dbContext.SaveChangesAsync(token);

        public async Task Update(T entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Reservations?> ReservationById(int id, CancellationToken token)
        {
           return await _dbContext.Set<Reservations>().Include(i => i.Cars)
            .Include(e => e.Customers)
                .ThenInclude(c => c.Adress)
            .Include(e => e.Customers)
                .ThenInclude(c => c.Gender)
            .Include(e => e.Customers)
                .ThenInclude(c => c.Role).ThenInclude(p => p.Permission).FirstOrDefaultAsync(x => x.Id == id,token);
        }

        public async Task<Customers?> FindCustomersByEmail(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Customers>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
