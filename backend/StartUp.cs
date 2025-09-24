using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;   // for WriteAsync
using System.Linq;                 // for Any()

namespace BookStore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

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
                endpoints.MapGet("/health", async context =>
                {
                    await context.Response.WriteAsync("Healthy");
                });
            });

            // Seed data
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BookContext>();
                if (!context.Books.Any())
                {
                    context.Books.AddRange(
                        new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 12.99m },
                        new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Price = 14.99m },
                        new Book { Title = "1984", Author = "George Orwell", Price = 13.99m },
                        new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Price = 11.99m }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
