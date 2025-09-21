using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Use In-Memory Database for testing
builder.Services.AddDbContext<BookContext>(options =>
    options.UseInMemoryDatabase("BookStoreDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookContext>();
    context.Database.EnsureCreated();

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

// Models
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
