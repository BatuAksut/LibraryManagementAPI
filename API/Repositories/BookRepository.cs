using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext context;

        public BookRepository(BookDbContext context)
        {
            this.context = context;
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> DeleteAsync(int id)
        {
            var bookToDelete = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookToDelete == null) return null;

            context.Books.Remove(bookToDelete);
            await context.SaveChangesAsync();
            return bookToDelete;
        }

        public async Task<List<Book>> GetAllAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            var books = context.Books.Include(b => b.Category).AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.Title.Contains(filterQuery));
                }
                else if (filterOn.Equals("Author", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.Author.Contains(filterQuery));
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                books = sortBy.ToLower() switch
                {
                    "title" => isAscending ? books.OrderBy(x => x.Title) : books.OrderByDescending(x => x.Title),
                    "author" => isAscending ? books.OrderBy(x => x.Author) : books.OrderByDescending(x => x.Author),
                    _ => books
                };
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await books.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await context.Books.Include(b => b.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Book?> UpdateAsync(int id, Book book)
        {
            var bookToUpdate = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookToUpdate == null) return null;

            bookToUpdate.Title = book.Title;
            bookToUpdate.Author = book.Author;
            bookToUpdate.CategoryId = book.CategoryId;

            await context.SaveChangesAsync();
            return bookToUpdate;
        }
    }
}
