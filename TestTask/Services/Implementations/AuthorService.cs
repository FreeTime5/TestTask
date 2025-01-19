using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    internal class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext dbContext;

        public AuthorService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Author> GetAuthor()
        {
            var maxLength = await dbContext.Books
                .AsNoTracking()
                .MaxAsync(b => b.Title.Length);

            return dbContext.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Where(b => b.Title.Length == maxLength)
                .Select(b => b.Author)
                .AsEnumerable()
                .MinBy(a => a.Id) 
                ?? throw new Exception("There are no books");
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await dbContext.Books
                .AsNoTracking()
                .Where(b => b.PublishDate >= new DateTime(2015, 1, 1))
                .GroupBy(b => b.Author)
                .Where(i => (i.Count() & 1) == 0)
                .Select(g => g.Key)
                .ToListAsync();
        }
    }
}
