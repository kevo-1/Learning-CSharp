using Movie.Application.Interfaces;

namespace Movie.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public IMovieRepository Movies {get;}
    public IReviewRepository Reviews {get;}

    public UnitOfWork(IMovieRepository movieRepo, IReviewRepository reviewRepo)
    {
        Movies = movieRepo;
        Reviews = reviewRepo;
    }
    public void SaveChanges()
    {
        Console.WriteLine("Changes Saved to both repositories!");
    }
}