
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace ProjectTrackingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          
            // Load configuration, including environment-specific files
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            
            //Add cors policy
            builder.Services.AddCors(options =>
            {
                    options.AddPolicy("AllowSpecificOrigin", policy => {
                        policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:5173")
                                          .AllowAnyMethod()
                                          .AllowAnyHeader();
                                      });
            });
            

            // Add services to the container.
            builder.Services.AddControllers();

            // Register the DbContext with dependency injection
            builder.Services.AddDbContext<Data.AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<Data.Repositories.IProjectsRepository, Data.Repositories.ProjectsRepository>();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
