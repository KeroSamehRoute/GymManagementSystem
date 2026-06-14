using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	[Authorize]
	public class HomeController(IAnalyticsService analyticsService) : Controller
	{
		private readonly IAnalyticsService _analyticsService = analyticsService;

        public async Task<IActionResult> Index(CancellationToken ct)
		{
			return View(await _analyticsService.GetAnalyticsDataAsync(ct));
        }
    }
}
