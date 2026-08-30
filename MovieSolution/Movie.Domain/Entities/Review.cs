namespace Movie.Domain.Entities;

public class Review
{
    public required int Id {set; get;}
    public required int MovieId {set; get;}
    public required string Reviewer {set; get;}
    public required int Rating {set; get;}
}