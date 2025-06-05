
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SongApiLb;

public class SongService : ISongService
{
    private readonly MongoClient _client;
    private readonly IMongoCollection<Song> _songsCollection;

    public SongService(IOptions<DatabaseSettings> options)
    {
        _client = new MongoClient(options.Value.ConnectionString);
        var database = _client.GetDatabase("M165P3Lb");
        _songsCollection = database.GetCollection<Song>("songs");
    }

    public void Create(Song song)
    {
        _songsCollection.InsertOne(song);
    }

    public IEnumerable<Song> Get()
    {
        return _songsCollection.Find(_ => true).ToList();
    }

    public Song Get(string id)
    {
        return _songsCollection.Find(s => s.Id == id).FirstOrDefault();
    }

    public string GetDatabases()
    {
        var databases = _client.ListDatabaseNames().ToList();
        return string.Join(", ", databases);
    }

    public void Remove(string id)
    {
        _songsCollection.DeleteOne(s => s.Id == id);
    }

    public void Update(string id, Song song)
    {
        song.Id = id;
        _songsCollection.ReplaceOne(s => s.Id == id, song);
    }
}
