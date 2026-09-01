namespace Movie.Application.Interfaces;

public interface IUnitOfWork
{
    IMovieRepository Movies { get; }
    IReviewRepository Reviews { get; }
    Task SaveChangesAsync();
}
