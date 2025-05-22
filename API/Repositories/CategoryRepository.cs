using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext context;

        public CategoryRepository(BookDbContext context)
        {
            this.context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var categoryToDelete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoryToDelete == null) return null;

            context.Categories.Remove(categoryToDelete);
            await context.SaveChangesAsync();
            return categoryToDelete;
        }

        public async Task<List<Category>> GetAllAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            var categories = context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    categories = categories.Where(x => x.Name.Contains(filterQuery));
                }
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    categories = isAscending
                        ? categories.OrderBy(x => x.Name)
                        : categories.OrderByDescending(x => x.Name);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;
            return await categories.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var categoryToUpdate = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categoryToUpdate == null) return null;

            categoryToUpdate.Name = category.Name;

            

            await context.SaveChangesAsync();
            return categoryToUpdate;
        }
    }
}
