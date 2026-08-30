namespace Movie.Infrastructure.Repositories;

using Movie.Domain.Entities;
using Movie.Application.Interfaces;

public class ReviewRepository : IReviewRepository
{
    private readonly List<Review> _reviews = new();
    public List<Review> GetAllReviews()
    {
        return _reviews;
    }
    public List<Review> GetMovieReviews(int movieId)
    {
        return _reviews.Where(r => r.MovieId == movieId).ToList();
    }
    public Review? GetReviewById(int reviewId)
    {
        if(reviewId <= 0)
            return null;
        var review = _reviews.FirstOrDefault(r => r.Id == reviewId);
        if(review is null)
            return null;
        return review;
    }
    public void PostReview(Review review)
    {
        if(review is null) return;
        _reviews.Add(review);
    }
    public bool UpdateReview(Review review)
    {
        if(review is null) return false;
        var fetchedRev = _reviews.FirstOrDefault(r => r.Id == review.Id);
        if(fetchedRev is null) return false;

        fetchedRev.MovieId = review.MovieId;
        fetchedRev.Rating = review.Rating;
        fetchedRev.Reviewer = review.Reviewer;
        return true;
    }
    public bool DeleteReview(int id)
    {
        if(id <= 0) return false;
        var review = _reviews.FirstOrDefault(r => r.Id == id);

        if(review is null) return false;
        _reviews.Remove(review);
        return true;
    }
}