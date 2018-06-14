using MembershipSystem.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MembershipSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add authentication 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = CustomAuthOptions.DefaultScheme;
            })
            // Call custom authentication extension method
            .AddCustomAuth(options =>
            {
                // Configure single or multiple passwords for authentication
                options.AuthKey = "custom auth key";
            });

            services.AddMvc(options =>
            {
                // All endpoints need authentication
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });

            bool UseDummyDb = Configuration.GetValue<bool>("UseDummyInMemory");
            Func<DbContextOptionsBuilder<MembersContext>, DbContextOptionsBuilder<MembersContext>> opt;
                opt = o => o.UseInMemoryDatabase("dummy_database");
                MembersController.options = opt(new DbContextOptionsBuilder<MembersContext>()).Options;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable authentication capabilities
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
