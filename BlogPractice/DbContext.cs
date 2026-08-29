using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

/*
When applying conflicted properties to attributes like different max lengths, 
the OnModelCreating always runs last, so any changes applied there have the final call on how the attribute is shaped
*/

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Post> Posts { set; get; }
    public DbSet<Blog> Blogs { set; get; }
    public DbSet<Author> Authors {set; get;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\TESTINGSQLSERVER;Database=BlogPracticeDb;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>().ToTable("Blog");
        modelBuilder.Entity<Post>().ToTable("Post");
        modelBuilder.Entity<Author>().ToTable("Author");
        modelBuilder.Entity<Blog>(b =>
        {
            b.HasKey(d => d.Id);
            b.Property(d => d.Name).IsRequired().HasMaxLength(100);
            b.Property(d => d.Url).IsRequired().HasMaxLength(1500);
            b.HasMany(b => b.Posts).WithOne(p => p.Blog).HasForeignKey(p => p.BlogId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Post>(p =>
        {
            p.HasKey(d => d.Id);
            p.Property(d => d.PublishedAt).IsRequired().HasColumnType("datetime");
            p.Property(d => d.Title).IsRequired().HasMaxLength(100);
            p.Property(d => d.Content).IsRequired().HasMaxLength(1500);
        });

        modelBuilder.Entity<Author>(a =>
        {
            a.HasKey(a => a.Id);
            a.Property(a => a.Email).IsRequired().HasMaxLength(250);
            a.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            a.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            a.Property(a => a.UserName).IsRequired().HasMaxLength(50);
            a.HasMany(a => a.Posts).WithOne(p => p.Author).HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public class Author
{
    public required int Id {set; get;}
    public required string FirstName {set; get;}
    public required string LastName {set; get;}
    public required string Email {set; get;}
    public required string UserName {set; get;}
    public List<Post> ?Posts {set; get;}
}

public class Post
{
    public required int Id { set; get; }
    public required string Title { set; get; }
    public required string Content { set; get; }
    public DateTime PublishedAt { set; get; }
    public required Author Author {set; get;}
    public required Blog Blog{set; get;}
    public required int AuthorId {set; get;}
    public required int BlogId {set; get;}
}

public class Blog
{
    public required int Id { set; get; }
    public required string Name { set; get; }
    public required string Url { set; get; }
    public List<Post> ?Posts{set; get;}
}
