using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using BookStore; // Replace with your namespace

namespace BookStore.Tests
{
    public class BookControllerTests
    {
        private BookContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new BookContext(options);

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
            using var context = GetInMemoryContext();
            var controller = new BooksController(context);

            var result = await controller.GetBooks();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Book>>>(result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
            Assert.Equal(2, books.Count());
        }

        // Repeat same pattern for other tests...
    }
}
