namespace Movie.Infrastructure;

using Movie.Domain.Entities;
using Microsoft.EntityFrameworkCore;


public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Review> Reviews => Set<Review>();
}