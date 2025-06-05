using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SongApiLb;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<ISongService, SongService>();

var app = builder.Build();

app.MapGet("/databases", (IOptions<DatabaseSettings> options) =>
{
    var client = new MongoClient(options.Value.ConnectionString);
    var databases = client.ListDatabaseNames().ToList();

    return Results.Ok($"Zugriff auf MongoDB ok. Vorhandene DBs: {string.Join(", ", databases)}");
});

app.MapGet("/api/databases", (ISongService songService) =>
{
    var databases = songService.GetDatabases();
    return Results.Ok($"Zugriff auf MongoDB ok. Vorhandene DBs: {databases}");
});

app.MapPost("/api/songs", (Song song, ISongService songService) =>
{
    songService.Create(song);
    return Results.Created($"/api/songs/{song.Id}", song);
});

app.MapGet("/api/songs", (ISongService songService) =>
{
    var songs = songService.Get();
    return Results.Ok(songs);
});

app.MapGet("/api/songs/{id}", (string id, ISongService songService) =>
{
    var song = songService.Get(id);
    return song is not null ? Results.Ok(song) : Results.NotFound();
});

app.MapPut("/api/songs/{id}", (string id, Song song, ISongService songService) =>
{
    var existingSong = songService.Get(id);
    if (existingSong is null)
    {
        return Results.NotFound();
    }

    songService.Update(id, song);
    return Results.Ok(song);
});

app.MapDelete("/api/songs/{id}", (string id, ISongService songService) =>
{
    var existingSong = songService.Get(id);
    if (existingSong is null)
    {
        return Results.NotFound();
    }

    songService.Remove(id);
    return Results.NoContent();
});

app.Run();
