namespace Movie.Infrastructure.Repositories;

using Movie.Domain.Entities;
using Movie.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ReviewRepository : IReviewRepository
{
    private readonly MovieDbContext _context;
    public ReviewRepository(MovieDbContext context)
    {
        _context = context;
    }
    public async Task<List<Review>> GetAllReviewsAsync() => await _context.Reviews.ToListAsync();
    public async Task<List<Review>> GetMovieReviewsAsync(int movieId) => await _context.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
    public async Task<Review?> GetReviewByIdAsync(int reviewId)
    {
        if(reviewId <= 0)
            return null;
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if(review is null)
            return null;
        return review;
    }
    public async Task PostReviewAsync(Review review)
    {
        if(review is null) return;
        await _context.Reviews.AddAsync(review);
    }
    public async Task<bool> UpdateReviewAsync(Review review)
    {
        if(review is null) return false;
        var fetchedRev = await GetReviewByIdAsync(review.Id);
        if(fetchedRev is null) return false;

        fetchedRev.MovieId = review.MovieId;
        fetchedRev.Rating = review.Rating;
        fetchedRev.Reviewer = review.Reviewer;
        return true;
    }
    public async Task<bool> DeleteReviewAsync(int id)
    {
        if(id <= 0) return false;
        var review = await GetReviewByIdAsync(id);

        if(review is null) return false;
        _context.Reviews.Remove(review);
        return true;
    }
}