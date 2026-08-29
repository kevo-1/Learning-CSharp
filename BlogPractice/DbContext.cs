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
        modelBuilder.Entity<Blog>(b =>
        {
            b.HasKey(d => d.Id);
            b.Property(d => d.Name).IsRequired().HasMaxLength(100);
            b.Property(d => d.Url).IsRequired().HasMaxLength(1500);
        });

        modelBuilder.Entity<Post>(p =>
        {
            p.HasKey(d => d.Id);
            p.Property(d => d.PublishedAt).IsRequired().HasColumnType("datetime");
            p.Property(d => d.Title).IsRequired().HasMaxLength(100);
            p.Property(d => d.Content).IsRequired().HasMaxLength(1500);
        });
    }
}

public class Post
{
    public required int Id { set; get; }
    public required string Title { set; get; }
    public required string Content { set; get; }
    public DateTime PublishedAt { set; get; }
}

public class Blog
{
    public required int Id { set; get; }
    [MaxLength(50)]
    public required string Name { set; get; }
    public required string Url { set; get; }
}
