using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using MediatR;

namespace Questao5
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddSwaggerGen();

            services.AddSingleton(sp =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                return new DatabaseConfig { Name = connectionString };
            });

            services.AddScoped(sp =>
            {
                var config = sp.GetRequiredService<DatabaseConfig>();
                var connection = new SqliteConnection(config.Name);
                connection.Open();
                return connection;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddScoped<IDatabaseBootstrap, DatabaseBootstrap>();
            services.AddScoped<ICommandStore, CommandStore>();
            services.AddScoped<IQueryStore, QueryStore>();

            services.AddScoped(provider =>
            {
                var dbBootstrap = provider.GetRequiredService<IDatabaseBootstrap>();
                dbBootstrap.Setup();
                return dbBootstrap;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
