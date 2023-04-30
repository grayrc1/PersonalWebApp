using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalWebApp.Data;
using Microsoft.Extensions.DependencyInjection;


namespace PersonalWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            connectionString = connectionString.Replace("{DB_PASSWORD_NEW}", Environment.GetEnvironmentVariable("DB_PASSWORD_NEW"));

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CanWriteBlogPosts", policy => policy.RequireClaim("CanWriteBlogPosts"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();



            var app = builder.Build();

            /*using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRolesAsync(roleManager);
            }*/
            

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        /*private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }
        }*/

    }
}