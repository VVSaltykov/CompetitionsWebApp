using CompetitionsWebApp.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CompetitionsWebApp
{
    public class StartUp
    {
        public IConfigurationRoot _confString;

        [Obsolete]
        public StartUp(IHostingEnvironment hostingEnvironment)
        {
            _confString = new ConfigurationBuilder().
                SetBasePath(hostingEnvironment.ContentRootPath).
                AddJsonFile("appsettings.json").
                Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            // Add services to the container.
            services.AddControllersWithViews(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.EnableEndpointRouting = false;
            });

            services.AddTransient<UserRepository>();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();



            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
