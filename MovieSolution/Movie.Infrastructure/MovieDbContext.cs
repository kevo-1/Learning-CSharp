namespace Movie.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Movie.Domain.Entities;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Review> Reviews => Set<Review>();
}
