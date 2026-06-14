using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository(GymDbContext dbContext) : GenericRepository<SessionEntity>(dbContext), ISessionRepository
    {
        private readonly GymDbContext _dbContext = dbContext;

        public async Task<IEnumerable<SessionEntity>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<SessionEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<SessionEntity> query = _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return _dbContext.Bookings
                .AsNoTracking()
                .CountAsync(b => b.SessionId == sessionId, ct);
        }

        public Task<SessionEntity?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default)
        {
            return _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == sessionId, ct);
        }

    }
}
