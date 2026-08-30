namespace Movie.Infrastructure.Repositories;

using Movie.Domain.Entities;
using Movie.Application.Interfaces;

public class MovieRepository: IMovieRepository
{
    private readonly List<Movie> _movies = new();

    public List<Movie> GetAll()
    {
        return _movies;
    }
    public Movie? GetById(int Id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == Id);
        return movie;
    }
    public void Add(Movie movie)
    {
        if(movie is null) return;
        _movies.Add(movie);
    }
    public bool Update(Movie movie)
    {
        if(movie is null) return false;
        var res = _movies.FirstOrDefault(m => m.Id == movie.Id);
        if(res is null) return false;
        res.Genre = movie.Genre;
        res.Title = movie.Title;
        res.ReleaseYear = movie.ReleaseYear;
        return true;
    }
    public bool Delete(int Id)
    {
        if(Id <= 0) return false;
        var movie = _movies.FirstOrDefault(m => m.Id == Id);
        if(movie is null) return false;
        _movies.Remove(movie);
        return true;
    }
}