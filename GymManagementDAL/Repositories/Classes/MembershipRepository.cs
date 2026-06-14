using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementDAL.Repositories.Classes
{
	public class MembershipRepository(GymDbContext dbContext) : GenericRepository<MembershipEntity>(dbContext), IMembershipRepository
	{
		private readonly GymDbContext _dbContext = dbContext;

        public async Task<List<MembershipEntity>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MembershipEntity, bool>>? predicate = null,
           CancellationToken ct = default)
        {
            IQueryable<MembershipEntity> query = _dbContext.Memberships.AsNoTracking().Include(m => m.Plan).Include(m => m.Member);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }

	}
}
