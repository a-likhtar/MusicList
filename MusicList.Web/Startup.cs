using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MusicList.Application.Misc;
using MusicList.Application.Services;
using MusicList.Application.Services.Contracts;
using MusicList.CrosscuttingConcerns;
using MusicList.DataAccess;
using MusicList.DataAccess.DbEntities.Auth;
using MusicList.DataAccess.Repositories;
using MusicList.DataAccess.Repositories.Contracts;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Vue2Spa
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
            var cnfg = Configuration.Get<AppSettings>();
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(cnfg.ConnectionStrings.IdentityConnection));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<ITracksRepository, TracksRepository>();
            services.AddScoped<ITracksService, TracksService>();

            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        // replace "token" with whatever your param name is
                        if (ctx.Request.Query.ContainsKey("token"))
                            ctx.Token = ctx.Request.Query["token"];
                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // set, validate publisher
                    ValidateIssuer = cnfg.AuthOptions.ValidateIssuer,
                    // set, publisher
                    ValidIssuer = cnfg.AuthOptions.Issuer,

                    // set, validate client (UI app)
                    ValidateAudience = cnfg.AuthOptions.ValidateAudience,
                    // set, client
                    ValidAudience = cnfg.AuthOptions.Audience,
                    // set, validate duration of the token
                    ValidateLifetime = cnfg.AuthOptions.ValidateLifetime,

                    // set, key
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(cnfg.AuthOptions.Key)),
                    // set, validate key
                    ValidateIssuerSigningKey = cnfg.AuthOptions.ValidateIssuerSigningKey

                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Music List Api", Version = "v1" });
                //c.OperationFilter<FileUploadOperation>();
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });
            });

            services.AddMvc(options =>
            {
                options.Conventions.Add(new ComplexTypeConvention());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Music List Api");
                });

                // Webpack initialization with hot-reload.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Music List Api");
                });

            }
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            //Create tables
            context.Database.EnsureCreated();
        }
    }
}
