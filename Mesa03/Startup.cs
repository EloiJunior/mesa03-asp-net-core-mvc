using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Mesa03.Models;
using Mesa03.Services; //para usar classe SellerService no registro do SellerService no sistema de injeção de dependencia
using System.Globalization;//para poder usar as funções de cultura
using Microsoft.AspNetCore.Localization; // para poder definir a localização da aplicação, embaixo no metodo Configure vamos definir como EUA que é a mais generica

namespace Mesa03
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<Mesa03Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Mesa03Context")));

            services.AddScoped<SellerService>(); //registrando serviço no sistema de injeção de dependencia
            services.AddScoped<DepartmentService>(); //registrando serviço no sistema de injeção de dependencia
            services.AddScoped<SalesRecordService>(); //registrando serviço no sistema de injeção de dependencia
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //definindo o Locale da aplicação como sendo dos EUA
            var enUS = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUS),  // qual vai ser o locale padrao da aplicação
                SupportedCultures = new List<CultureInfo> { enUS }, //quais são os locales possiveis da aplicação
                SupportedUICultures = new List<CultureInfo> { enUS } 

            };

            app.UseRequestLocalization(localizationOptions);
            //
       
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
