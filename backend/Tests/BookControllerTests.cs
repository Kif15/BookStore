using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class BookControllerTests
{
    private BookContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<BookContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new BookContext(options);
        
        // Seed test data
        context.Books.AddRange(
            new Book { Id = 1, Title = "Test Book 1", Author = "Test Author 1", Price = 10.99m },
            new Book { Id = 2, Title = "Test Book 2", Author = "Test Author 2", Price = 15.99m }
        );
        context.SaveChanges();
        
        return context;
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var controller = new BooksController(context);

        // Act
        var result = await controller.GetBooks();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Book>>>(result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
        Assert.Equal(2, books.Count());
    }

    [Fact]
    public async Task GetBook_WithValidId_ReturnsBook()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var controller = new BooksController(context);

        // Act
        var result = await controller.GetBook(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Book>>(result);
        var book = Assert.IsType<Book>(actionResult.Value);
        Assert.Equal("Test Book 1", book.Title);
    }

    [Fact]
    public async Task GetBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var controller = new BooksController(context);

        // Act
        var result = await controller.GetBook(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostBook_WithValidBook_CreatesBook()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var controller = new BooksController(context);
        var newBook = new Book 
        { 
            Title = "New Test Book", 
            Author = "New Test Author", 
            Price = 20.99m 
        };

        // Act
        var result = await controller.PostBook(newBook);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Book>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var book = Assert.IsType<Book>(createdAtActionResult.Value);
        Assert.Equal("New Test Book", book.Title);
        
        // Verify it was saved to database
        var savedBook = await context.Books.FindAsync(book.Id);
        Assert.NotNull(savedBook);
        Assert.Equal("New Test Book", savedBook.Title);
    }

    [Fact]
    public async Task DeleteBook_WithValidId_RemovesBook()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var controller = new BooksController(context);

        // Act
        var result = await controller.DeleteBook(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        
        // Verify it was removed from database
        var deletedBook = await context.Books.FindAsync(1);
        Assert.Null(deletedBook);
    }

    [Fact]
    public async Task DeleteBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var controller = new BooksController(context);

        // Act
        var result = await controller.DeleteBook(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}