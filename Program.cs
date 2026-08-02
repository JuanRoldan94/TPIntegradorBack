using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TPIntegradorBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.LogoutPath = "/Login/Logout";
                    options.AccessDeniedPath = "/Login/AccesoDenegado";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
