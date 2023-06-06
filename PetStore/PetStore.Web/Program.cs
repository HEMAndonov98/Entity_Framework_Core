using Microsoft.EntityFrameworkCore;
using PetStore.Data;
using PetStore.Data.Common.Repos;
using PetStore.Data.Models;
using PetStore.Data.Repositories;
using PetStore.Services.Data;
using PetStore.Services.Mapping;

namespace PetStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = ConfigureWebApplication(args);

            app.Run();
        }

        private static WebApplication ConfigureWebApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            
            //Db Context
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            
            //Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    IdentityOptionsProvider.GetIdentityOptions(options);
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc();
            
            //Data Repos
            builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            
            
            
            //Automapper

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(PetStoreProfile));
            });
            
            //Services
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
            app.MapRazorPages();
            return app;
        }
    }
}