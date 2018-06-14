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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = CustomAuthOptions.DefaultScheme;
            })
            .AddCustomAuth(options =>
            {
                options.AuthKey = "custom auth key";
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });

            Func<DbContextOptionsBuilder<MembersContext>, DbContextOptionsBuilder<MembersContext>> opt;
            bool UseDummyDb = Configuration.GetValue<bool>("UseDummyInMemory");
            if (UseDummyDb)
            {
                opt = o => o.UseInMemoryDatabase("dummy_database");
            } else
            {
                opt = o => o.UseSqlServer(Configuration.GetValue<string>("DefaultConnection"));
            }
            DbContextOptions<MembersContext>  contextOptions = opt(new DbContextOptionsBuilder<MembersContext>()).Options;
            MembersController.options = contextOptions;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
