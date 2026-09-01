namespace Movie.Domain.Entities;

public class Movie
{
    public int Id { set; get; }
    public required string Title { set; get; }
    public required string Genre { set; get; }
    public required int ReleaseYear { set; get; }
    public List<Review> Reviews { get; private set; } = new();

    public void AddReview(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5");

        review.MovieId = this.Id;
        Reviews.Add(review);
    }
}

public record MovieCreateDto(string Title, string Genre, int ReleaseYear);
