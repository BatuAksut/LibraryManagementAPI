using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BooksAuthDbContext : IdentityDbContext
    {
        public BooksAuthDbContext(DbContextOptions<BooksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Reader",
                    NormalizedName = "READER"
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Writer",
                    NormalizedName = "WRITER"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
