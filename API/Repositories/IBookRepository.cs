using API.Models;

namespace API.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Book?> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book book);
        Task<Book?> UpdateAsync(int id, Book book);
        Task<Book?> DeleteAsync(int book);
    }
}
