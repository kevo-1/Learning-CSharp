using Microsoft.EntityFrameworkCore;
using Movie.Api.Middlewares;
using Movie.Application.Interfaces;
using Movie.Domain.Entities;
using Movie.Infrastructure;
using Movie.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDb"))
);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/movies", async (IMovieRepository repo) => await repo.GetAllAsync());

app.MapPost(
    "/movies",
    async (IUnitOfWork uow, MovieCreateDto movieDto) =>
    {
        var movie = new Movie.Domain.Entities.Movie
        {
            Title = movieDto.Title,
            Genre = movieDto.Genre,
            ReleaseYear = movieDto.ReleaseYear,
        };
        await uow.Movies.AddAsync(movie);
        await uow.SaveChangesAsync();
        return Results.Created($"/movies/{movie.Id}", movie);
    }
);

app.MapPost(
    "/movies/{id}/reviews",
    async (int id, IUnitOfWork uow, ReviewCreateDto reviewDto) =>
    {
        if (id <= 0)
            return Results.BadRequest();

        var movie = await uow.Movies.GetByIdAsync(id);
        if (movie is null)
            return Results.NotFound();
        var review = new Review
        {
            MovieId = id,
            Reviewer = reviewDto.Reviewer,
            Rating = reviewDto.Rating,
        };

        try
        {
            movie.AddReview(review);
        }
        catch (ArgumentException e)
        {
            return Results.BadRequest(e.Message);
        }

        await uow.SaveChangesAsync();
        return Results.Created($"/movies/{id}/reviews", review);
    }
);

app.Run();
