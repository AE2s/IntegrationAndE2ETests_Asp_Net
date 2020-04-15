using Microsoft.EntityFrameworkCore;
using MVCAsyncAwait.Models;

namespace MVCAsyncAwait
{
    public class MovieContext : DbContext
    {
        public MovieContext()
        {
           
        }
        public MovieContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}