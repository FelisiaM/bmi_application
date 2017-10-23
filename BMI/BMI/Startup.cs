using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BMI.Authorisation;
using BMI.Reporting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace BMI
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
            // Add application services
            services.AddTransient<IBmiReport, BmiReport>();
            services.AddTransient<ICsvReader, CsvReader>();
            services.AddTransient<ITokenHandler, TokenHandler>();

            // enforce all requests to use ssl - global config
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add framework services
            services.AddMvc();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.Authority = Configuration["Authority"];
                    options.ClientId = Configuration["Client"];
                    options.CallbackPath = Configuration["CallbackPath"];

                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.FormPost;

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.RequireHttpsMetadata = false;
                    options.MetadataAddress = Configuration["MetadataAddress"];

                    options.SaveTokens = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Redirects all HTTP requests to HTTPS:
            app.UseRewriter(
                new RewriteOptions()
                .AddRedirectToHttps());
            
            app.UseAuthentication();
        }
    }
}
