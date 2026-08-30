using Movie.Application.Interfaces;
using Movie.Domain.Entities;
using Movie.Infrastructure;
using Movie.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMovieRepository, MovieRepository>();
builder.Services.AddSingleton<IReviewRepository, ReviewRepository>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();


var app = builder.Build();

app.MapGet("/movies", (IMovieRepository repo) => repo.GetAll());

app.MapPost("/movies/{id}/reviews", (int id, IUnitOfWork uow, Review review) =>
{
    if(id <= 0) 
        return Results.BadRequest();

    var movie = uow.Movies.GetById(id);
    if(movie is null) 
        return Results.NotFound();
    uow.Reviews.PostReview(review);
    uow.SaveChanges();
    return Results.Created();
});

app.Run();
