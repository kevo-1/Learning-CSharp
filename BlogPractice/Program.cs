using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


using var db = new AppDbContext();


var blog = new Blog { Id = 0, Name = "Tech Notes", Url = "https://example.com" };
var author = new Author { Id = 0, FirstName = "Sam", LastName = "Reyes", Email = "sam@example.com", UserName = "sreyes" };

db.Blogs.Add(blog);
db.Authors.Add(author);
db.SaveChanges();

var post = new Post
{
    Id = 0,
    Title = "First Post",
    Content = "Hello world",
    PublishedAt = DateTime.Now,
    Blog = blog,       
    Author = author,   
    BlogId = blog.Id,
    AuthorId = author.Id
};

db.Posts.Add(post);
db.SaveChanges();

var authorWithPosts = db.Authors.Include(a => a.Posts).First(a => a.Id == author.Id);

Console.WriteLine($"{authorWithPosts.FirstName} wrote:");
foreach (var p in authorWithPosts.Posts!)
{
    Console.WriteLine($"  - {p.Title}");
}

var postWithAuthor = db.Posts.Include(p => p.Author).Include(p => p.Blog).First(p => p.Id == post.Id);

Console.WriteLine($"\"{postWithAuthor.Title}\" was written by {postWithAuthor.Author.FirstName} on the blog \"{postWithAuthor.Blog.Name}\"");