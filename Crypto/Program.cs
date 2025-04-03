using Crypto.Identity;
using Crypto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Crypto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();


            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true; //rakan zorunlu
                options.Password.RequireLowercase = true; //k���k harf zorunlu 
                options.Password.RequireUppercase = true; //b�y�k harf zorunlu
                options.Password.RequireNonAlphanumeric = true; //rakam ve alfabe d���nda kalan �zel karakter zorunlu
                options.Password.RequiredLength = 5; //min �ifre uzunlu�u

                options.Lockout.MaxFailedAccessAttempts = 5; //max hatal� giri� say�s�
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);//max hatal� giri� sonras� account kitleme s�resi
                options.Lockout.AllowedForNewUsers = true; //her yeni account i�in uygula 

                //user
                options.User.RequireUniqueEmail = true; //account un email adresi benzersiz

                options.SignIn.RequireConfirmedEmail = false; //giri� i�in email onay� zorunlu
                options.SignIn.RequireConfirmedPhoneNumber = false; //giri� i�in telefon numaras� onay� zorunlu 
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5); // oturum s�resi bu
                options.SlidingExpiration = true; //her kullan�c� hareketinde oturum s�resini yenile 
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name="Crypto.Security.Cookie",
                    SameSite= SameSiteMode.Strict
                };
            } 
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
