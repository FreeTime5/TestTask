using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    internal class BookService : IBookService
    {
        private readonly DateTime CarolusRexPublishingDate = new DateTime(2012, 5, 25);
        private readonly ApplicationDbContext dbContext;

        public BookService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Book> GetBook()
        {
            return dbContext.Books
                .AsNoTracking()
                .AsEnumerable()
                .MaxBy(b => b.Price) ?? throw new Exception("Book is missing");
        }

        public async Task<List<Book>> GetBooks()
        {
            return await dbContext.Books
                .AsNoTracking()
                .Where(b => b.Title.Contains("Red") && b.PublishDate > CarolusRexPublishingDate)
                .ToListAsync();
        }
    }
}
