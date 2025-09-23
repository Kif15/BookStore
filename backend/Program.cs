using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS with specific frontend origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add Entity Framework with IN-MEMORY database
builder.Services.AddDbContext<BookContext>(options =>
    options.UseInMemoryDatabase("BookStoreDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS (must be before UseAuthorization)
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Add health check endpoint
app.MapGet("/health", () => "Healthy");

// Seed data
using (var scope = app.Services.CreateScope())
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

app.Run();

// Models (keep your existing Book and BookContext classes)
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public decimal Price { get; set; }
}

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options) { }
    public DbSet<Book> Books { get; set; }
}