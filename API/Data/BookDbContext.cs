using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
        .HasOne(b => b.Category)
        .WithMany(c => c.Books)
        .HasForeignKey(b => b.CategoryId)
        .OnDelete(DeleteBehavior.Cascade);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Roman" },
                new Category { Id = 2, Name = "Bilim" },
                new Category { Id = 3, Name = "Tarih" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Simyacı",
                    Author = "Paulo Coelho",
                    CategoryId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Sapiens",
                    Author = "Yuval Noah Harari",
                    CategoryId = 3
                },
                new Book
                {
                    Id = 3,
                    Title = "Kısa Cevaplar",
                    Author = "Stephen Hawking",
                    CategoryId = 2
                }
            );
        }

    }
}
