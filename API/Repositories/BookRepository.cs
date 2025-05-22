using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BookRepository:IBookRepository
    {
        private readonly BookDbContext context;

        public BookRepository(BookDbContext context) {
            this.context = context;
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return book;
        }

        public Task<Book?> DeleteAsync(int book)
        {
            var bookToDelete = context.Books.FirstOrDefaultAsync(x => x.Id == book);
            if (bookToDelete == null) return Task.FromResult<Book?>(null);
            context.Books.Remove(bookToDelete.Result);
            context.SaveChangesAsync();
            return bookToDelete;
        }

        public async Task<List<Book>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var books = context.Books.Include(b => b.Category).AsQueryable();





            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    books = books.Where(x => x.Title.Contains(filterQuery));
                }

            }



            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    books = isAscending ? books.OrderBy(x => x.Title) : books.OrderByDescending(x => x.Title);
                }
                else if (sortBy.Equals("Author", StringComparison.OrdinalIgnoreCase))
                {
                    books = isAscending ? books.OrderBy(x => x.Author) : books.OrderByDescending(x => x.Author);
                }

            }



            var skipResults = (pageNumber - 1) * pageSize;

            return await books.Skip(skipResults).Take(pageSize).ToListAsync();

        }

        public Task<Book?> GetByIdAsync(int id)
        {
            var book = context.Books.Include(b => b.Category).FirstOrDefaultAsync(x => x.Id == id);
            return book;
        }

        public Task<Book?> UpdateAsync(int id, Book book)
        {
           var bookToUpdate= context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookToUpdate == null) return Task.FromResult<Book?>(null);

            bookToUpdate.Result.Title = book.Title;
            bookToUpdate.Result.Author = book.Author;
            bookToUpdate.Result.CategoryId = book.CategoryId;
            context.SaveChangesAsync();
            return bookToUpdate;
        }
    }
}
