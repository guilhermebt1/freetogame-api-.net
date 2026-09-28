using System.Text.Json.Serialization;

namespace api_freetogame.DTOs;

public class GameDto
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("thumbnail")]
    public required string Thumbnail { get; set; }
    [JsonPropertyName("short_description")]
    public required string ShortDescription { get; set; }
    [JsonPropertyName("game_url")]
    public string GameUrl { get; set; }
    [JsonPropertyName("genre")]
    public required string Genre { get; set; }
    [JsonPropertyName("platform")]
    public required string Platform { get; set; }
    [JsonPropertyName("developer")]
    public required string Developer { get; set; }
    [JsonPropertyName("publisher")]
    public string Publisher { get; set; }
    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }
    
}