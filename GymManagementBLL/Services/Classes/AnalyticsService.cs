using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService(IUnitOfWork unitOfWork) : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var upcomingSessions = 
                await _unitOfWork.GetRepository<SessionEntity>().CountAsync(s => s.StartDate > now, ct);

            var ongoingSessions = 
                await _unitOfWork.GetRepository<SessionEntity>().CountAsync(X => X.StartDate <= now && X.EndDate >= now, ct);

            var completedSessions = 
                await _unitOfWork.GetRepository<SessionEntity>().CountAsync(X => X.EndDate < now, ct);

            var totalMembers = 
                await _unitOfWork.GetRepository<MemberEntity>().CountAsync(ct: ct);

            var totalTrainers = 
                await _unitOfWork.GetRepository<TrainerEntity>().CountAsync(ct: ct);

            var activeMembers = 
                await _unitOfWork.GetRepository<MembershipEntity>().CountAsync(m => m.EndDate > now, ct);

            return new AnalyticsViewModel()
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };

        }

    }
}
