using Afelio_DemoCQRS.Dal.Commands;
using Afelio_DemoCQRS.Dal.Entities;
using Afelio_DemoCQRS.Dal.Queries;
using System.Data.Common;
using System.Data.SqlClient;
using Tools.CQRS;
using Tools.CQRS.Commands;
using Tools.CQRS.Queries;

namespace Afelio_DemoCQRS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IConfiguration configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region Configuration des services de sessions
            builder.Services.AddMemoryCache();
            builder.Services.AddSession(o =>
            {
                o.Cookie.Name = "AfelioDemoCQRS.cookie";
                o.Cookie.HttpOnly = true;
                o.IOTimeout = TimeSpan.FromMinutes(10);
            });
            #endregion

            builder.Services.AddTransient<DbConnection>(sp => new SqlConnection(configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<Dispatcher>();
            builder.Services.AddHandlers();


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

            #region Activation des sessions
            app.UseSession();
            #endregion

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}