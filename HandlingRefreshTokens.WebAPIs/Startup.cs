using System;
using System.Text;
using HandlingRefreshTokens.WebAPIs.DbContexts.Entities;
using HandlingRefreshTokens.WebAPIs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using HandlingRefreshTokens.WebAPIs.DbContexts;

namespace HandlingRefreshTokens.WebAPIs
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
           
            services.AddCors();
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddDbContext<LocalDbContext>(x => x.UseInMemoryDatabase("TempMemoryDb"));


            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IAuthService, AuthService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LocalDbContext context)
        {
            context.People.Add(new Person { Id = 1, FullName = "John Lennon" });
            context.People.Add(new Person { Id = 2, FullName = "Paul McCartney" });
            context.People.Add(new Person { Id = 3, FullName = "Ringo Starr" });
            context.People.Add(new Person { Id = 4, FullName = "George Harrison" });
            context.People.Add(new Person { Id = 5, FullName = "Pete Best" });
            context.People.Add(new Person { Id = 6, FullName = "Jimmie Nicol" });

            context.Users.Add(new User { Username = "test", Password = "test", IdUser = 1 });
            context.SaveChanges();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
