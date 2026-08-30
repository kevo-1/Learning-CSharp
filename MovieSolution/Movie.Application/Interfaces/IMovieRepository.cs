namespace Movie.Application.Interfaces;
using Movie.Domain.Entities;

public interface IMovieRepository
{
    public List<Movie> GetAll();
    public Movie? GetById(int Id);
    public void Add(Movie movie);
    public bool Update(Movie movie);
    public bool Delete(int Id);
}
