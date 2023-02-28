using LeBook.Models;
using Microsoft.EntityFrameworkCore;

namespace LeBook.DataAccess;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<CoverType> CoverTypes { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Age> Ages { get; set; }
    public DbSet<Book> Books { get; set; }
}