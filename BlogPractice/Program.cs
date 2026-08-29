public class App
{
    public static void Main(string[] args)
    {
        var longName = new string('x', 200);

        using var db = new AppDbContext();

        var blog = new Blog
        {
            Id = 0,
            Name = longName,
            Url = "https://example.com",
        };

        db.Blogs.Add(blog);

        try
        {
            db.SaveChanges();
            Console.WriteLine("Saved Successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed: {ex.GetType().Name}");
            Console.WriteLine(ex.InnerException?.Message);
        }
    }
}
