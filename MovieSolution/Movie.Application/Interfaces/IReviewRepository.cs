namespace Movie.Application.Interfaces;

using Movie.Domain.Entities;

public interface IReviewRepository
{
    public Task<List<Review>> GetAllReviewsAsync();
    public Task<List<Review>> GetMovieReviewsAsync(int movieId);
    public Task<Review?> GetReviewByIdAsync(int reviewId);
    public Task PostReviewAsync(Review review);
    public Task<bool> UpdateReviewAsync(Review review);
    public Task<bool> DeleteReviewAsync(int id);
}
