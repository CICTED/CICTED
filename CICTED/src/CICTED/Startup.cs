using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.Infrastucture.Contexts;
using Microsoft.EntityFrameworkCore;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.Infrastucture.Services;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Infrastucture.Repository;

namespace CICTED
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"], option => option.MigrationsAssembly("CICTED"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole<long>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext, long>()
                .AddDefaultTokenProviders();

            // Configure MyOptions using config by installing Microsoft.Extensions.Options.ConfigurationExtensions
            services.Configure<CustomSettings>(Configuration);

            services.Configure<CustomSettings>(myOptions =>
            {
                myOptions.ConnectionString = Configuration["ConnectionString"];
                myOptions.TwillioAccountSID = Configuration["TwillioAccountSID"];
                myOptions.TwillioNumber = Configuration["TwillioNumber"];
                myOptions.TwillioToken = Configuration["TwillioToken"];
                myOptions.TwillioURL = Configuration["TwillioURL"];
            });
            services.AddTransient<ILocalizacaoRepository, LocalizacaoRepository>();
            services.AddTransient<ITrabalhoRepository, TrabalhoRepository>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();
            services.AddTransient<IAreaRepository, AreaRepository>();
            services.AddTransient<IAutorRepository, AutorRepository>();
            services.AddTransient<IAgenciaRepository, AgenciaRepository>();
            services.AddTransient<IAdministradorRepository, AdministradorRepository>();
            services.AddTransient<IAvaliacaoRepository, AvaliacaoRepository>();
            services.AddTransient<IOrganizadorRepository, OrganizadorRepository>();
            services.AddTransient<IOrganizadorServices, OrganizadorServices>();
            services.AddTransient<ILocalizacaoServices, LocalizacaoServices>();

            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}");
            });
        }
    }
}
