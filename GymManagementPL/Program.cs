using GymManagementBLL;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<GymDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
			builder.Services.AddScoped<ISessionRepository, SessionRepository>();
			builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddScoped<IMemberService, MemberService>();
			builder.Services.AddScoped<ITrainerService, TrainerService>();
			builder.Services.AddScoped<IPlanService, PlanService>();
			builder.Services.AddScoped<ISessionService, SessionService>();
			builder.Services.AddScoped<IMembershipService, MembershipService>();
			builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
			builder.Services.AddScoped<IBookingService, BookingService>();
			builder.Services.AddScoped<IAttachmentService, AttachmentService>();

			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
			{
				Config.User.RequireUniqueEmail = true;
                Config.Lockout.MaxFailedAccessAttempts = 5;
                Config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

            }).AddEntityFrameworkStores<GymDbContext>();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";
			});

			var app = builder.Build();

			await app.MigrateAndSeedAsync();

            if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

            app.MapStaticAssets();

            app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=Login}/{id?}");
            await app.RunAsync();

        }
	}

}
