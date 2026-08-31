using Movie.Application.Interfaces;

namespace Movie.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieDbContext _dbContext;

    public IMovieRepository Movies { get; }
    public IReviewRepository Reviews { get; }

    public UnitOfWork(MovieDbContext context, IMovieRepository movies, IReviewRepository reviews)
    {
        _dbContext = context;
        Movies = movies;
        Reviews = reviews;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}