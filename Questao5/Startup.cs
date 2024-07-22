using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Microsoft.Data.Sqlite;
using System.Data;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Sqlite;

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

        services.AddSingleton<IDbConnection>(sp =>
        {
            var connection = new SqliteConnection(Configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            return connection;
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
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = string.Empty;
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
