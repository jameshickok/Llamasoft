namespace BakeryExercise.Server
{
    using AspNet.Security.OAuth.GitHub;
    using BakeryExercise.EntityFramework;
    using BakeryExercise.Services;
    using Bazinga.AspNetCore.Authentication.Basic;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OAuth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication<BasicCredentialProvider>(options => {
                    options.Events = new BasicAuthenticationEvents {
                        OnCredentialsValidated = async (context) =>
                        {
                            string name = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                            var bakeryContext = context.HttpContext.RequestServices.GetRequiredService<BakeryContext>();
                            var user = await bakeryContext.Users.FirstOrDefaultAsync(u => u.UserName == name);
                            var newClaims = new List<Claim>
                            {
                                new Claim(BakeryClaimTypes.BakeryAdmin, user.IsAdmin.ToString()),
                                new Claim(BakeryClaimTypes.BakeryUid, user.UserId.ToString())
                            };

                            context.Principal.AddIdentity(new ClaimsIdentity(newClaims));
                        }
                    };
                });

            services
                .AddDbContext<BakeryContext>(
                    dbOptions => dbOptions.UseSqlite("Data Source=bakery.db"));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.Requirements.Add(new AdminRequirement()));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Bakery API", Version = "v1" });

                c.MapType<Guid>(() => new Schema { Type = "string", Format = "uuid", Example = Guid.Empty });
                c.MapType<Guid?>(() => new Schema { Type = "string", Format = "uuid", Example = Guid.Empty });

                c.AddSecurityDefinition("basic", new BasicAuthScheme());
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                //    { "basic", new string[] { } }
                //});

                c.OperationFilter<SecurityFilter>();
            });

            services.AddSingleton<IAuthorizationHandler, AdminHandler>();

            services
                .AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        
            app.UseAuthentication();
            app.UseMvc();
        }

        public class SecurityFilter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                if (context.MethodInfo.GetCustomAttributesData().Any(i => i.AttributeType == typeof(AuthorizeAttribute)))
                {
                    if (operation.Security == null)
                    {
                        operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                    }

                    operation.Security.Add(new Dictionary<string, IEnumerable<string>> {
                        { "basic", new string[] { } }
                    });
                }
            }
        }
    }
}
