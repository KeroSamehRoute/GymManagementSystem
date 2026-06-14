using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class BookingRepository(GymDbContext dbContext) : GenericRepository<BookingEntity>(dbContext), IBookingRepository
    {
        private readonly GymDbContext _dbContext = dbContext;

        public Task<List<BookingEntity>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            return _dbContext.Bookings.AsNoTracking().Include(b => b.Member).Where(b => b.SessionId == sessionId).ToListAsync(ct);
        }

    }
}
