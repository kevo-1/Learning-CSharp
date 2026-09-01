namespace Movie.Domain.Entities;

public class Review
{
    public int Id { set; get; }
    public int MovieId { set; get; }
    public required string Reviewer { set; get; }
    public required int Rating { set; get; }
}

public record ReviewCreateDto(string Reviewer, int Rating);
