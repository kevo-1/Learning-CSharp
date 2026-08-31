namespace Movie.Application.Interfaces;
using Movie.Domain.Entities;

public interface IMovieRepository
{
    public Task<List<Movie>> GetAllAsync();
    public Task<Movie?> GetByIdAsync(int Id);
    public Task AddAsync(Movie movie);
    public Task<bool> UpdateAsync(Movie movie);
    public Task<bool> DeleteAsync(int Id);
}
