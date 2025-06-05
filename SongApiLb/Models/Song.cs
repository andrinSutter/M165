using MongoDB.Bson.Serialization.Attributes;

namespace SongApiLb;

public class Song
{
    [BsonId]
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string Genre { get; set; } = "";
    public string[] Artists { get; set; }= [];
}
