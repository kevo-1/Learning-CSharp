namespace Movie.Domain.Entities;

public class Movie
{
    public required int Id {set; get;}
    public required string Title {set; get;}
    public required string Genre {set; get;}
    public required int ReleaseYear {set; get;} 
}
