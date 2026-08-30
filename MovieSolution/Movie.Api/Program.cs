using Movie.Application.Interfaces;
using Movie.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMovieRepository, MovieRepository>();

var app = builder.Build();

app.MapGet("/movies", (IMovieRepository repo) => repo.GetAll());

app.Run();
