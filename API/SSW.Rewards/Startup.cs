using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using SSW.Rewards.Application;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Infrastructure;
using SSW.Rewards.Persistence;
using SSW.Rewards.WebAPI.Services;
using SSW.Rewards.WebAPI.Settings;
using Swashbuckle.AspNetCore.Swagger;

namespace SSW.Rewards
{
    public class Startup
	{
		public Startup(IWebHostEnvironment environment, IConfiguration configuration)
		{
			Environment = environment;
			Configuration = configuration;
		}

		public IWebHostEnvironment Environment { get; }
		public IConfiguration Configuration { get; }

        readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
		{
            // Configure all the stuffs
            ConfigureSettings(services);
			ConfigureSecrets(services);
			ConfigureLogging(services);

			services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
				.AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));

            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);
            services.AddApplication();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddApplicationInsightsTelemetry();
            services.AddDistributedMemoryCache();

            services.AddOpenApiDocument(d =>
            {
                d.DocumentName = "SSW.Rewards API";
                d.Version = "1.0";
                d.Description = "API Specification for the SSW Rewards mobile app.";
                d.Title = "SSW.Rewards API";
            });

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddCors(options => 
            {
                options.AddPolicy(AllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://sswconsultingdevh2krk.z8.web.core.windows.net")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
        }

		public virtual void ConfigureLogging(IServiceCollection services)
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.Enrich.FromLogContext()
				.Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
#if DEBUG
				// Used to filter out potentially bad data due debugging.
				// Very useful when doing Seq dashboards and want to remove logs under debugging session.
				.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached)
#endif
				.CreateLogger();
		}

		protected virtual void ConfigureSecrets(IServiceCollection services)
		{
			// Add Secrets
			// TODO: Perhaps add some registration via convention for anything that implements a nested ISecrets interface
			services.AddSingleton<SSWRewardsDbContext.ISecrets, Secrets>();
			services.AddSingleton<CloudBlobClientProvider.ISecrets, Secrets>();
		}

		protected virtual void ConfigureSettings(IServiceCollection services)
		{
			// Add Settings
			// TODO: Perhaps add some registration via convention for anything that implements a nested ISettings interface
			services.AddSingleton<KeyVaultSecretsProvider.ISettings, AppSettings>();
            services.AddSingleton<IWWWRedirectSettings, AppSettings>();
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();
			app.UseAuthorization();
			
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}