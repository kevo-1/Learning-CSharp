namespace Movie.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Movie.Application.Interfaces;
using Movie.Domain.Entities;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _context;

    public MovieRepository(MovieDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetAllAsync() =>
        await _context.Movies.Include(m => m.Reviews).ToListAsync();

    public async Task<Movie?> GetByIdAsync(int Id) =>
        await _context.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == Id);

    public async Task AddAsync(Movie movie)
    {
        _context.Movies.Add(movie);
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        if (movie is null)
            return false;
        var existing = await GetByIdAsync(movie.Id);
        if (existing is null)
            return false;
        existing.Genre = movie.Genre;
        existing.Title = movie.Title;
        existing.ReleaseYear = movie.ReleaseYear;
        return true;
    }

    public async Task<bool> DeleteAsync(int Id)
    {
        if (Id <= 0)
            return false;
        var movie = await GetByIdAsync(Id);
        if (movie is null)
            return false;
        _context.Movies.Remove(movie);
        return true;
    }
}
