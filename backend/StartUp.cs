using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStore
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
            services.AddControllers();
            services.AddSwaggerGen();

            // CORS 
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:300") // Frontend URL
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // In-Memory Database
            services.AddDbContext<BookContext>(options =>
                options.UseInMemoryDatabase("BookStoreDB"));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Health check endpoint
                endpoints.MapGet("/health", async context =>
                {
                    await context.Response.WriteAsync("Healthy");
                });
            });

            // Seed initial data
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BookContext>();

                if (!context.Books.Any())
                {
                    context.Books.AddRange(
                        new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 10.99M },
                        new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Price = 7.99M },
                        new Book { Title = "1984", Author = "George Orwell", Price = 8.99M },
                        new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Price = 11.99M }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
 }