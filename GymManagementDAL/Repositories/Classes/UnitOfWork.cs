using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork(GymDbContext dbContext,
        IMembershipRepository membershipRepository,
        ISessionRepository sessionRepository,
        IBookingRepository bookingRepository) : IUnitOfWork
    {
        public IMembershipRepository MembershipRepository { get; } = membershipRepository;
        public ISessionRepository SessionRepository { get; } = sessionRepository;
        public IBookingRepository BookingRepository { get; } = bookingRepository;

        private readonly Dictionary<string, object> repositories = [];
        private readonly GymDbContext _dbContext = dbContext;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var typeName = typeof(TEntity).Name;

            if (repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<TEntity>)value;

            var Repo = new GenericRepository<TEntity>(_dbContext);

            repositories[typeName] = Repo;

            return Repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _dbContext.SaveChangesAsync(ct);
        }

    }
}
