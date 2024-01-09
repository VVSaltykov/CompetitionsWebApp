using CompetitionsWebApp.API.Interfaces;
using CompetitionsWebApp.API.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddTransient<IUserRepository, UserRepository>();

            // Add services to the container.
            services.AddControllersWithViews(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.EnableEndpointRouting = false;
            });
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
