namespace Movie.Application.Interfaces;

using Movie.Domain.Entities;

public interface IReviewRepository{
    public List<Review> GetAllReviews();
    public List<Review> GetMovieReviews(int movieId);
    public Review? GetReviewById(int reviewId);
    public void PostReview(Review review);
    public bool UpdateReview(Review review);
    public bool DeleteReview(int id);
}