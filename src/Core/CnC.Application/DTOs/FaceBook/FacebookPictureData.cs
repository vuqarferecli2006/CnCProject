using System.Text.Json.Serialization;

namespace CnC.Application.DTOs.FaceBook;

public record FacebookPictureData
{
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("is_silhouette")]
    public bool IsSilhouette { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }=null!;

    [JsonPropertyName("width")]
    public int Width { get; set; }
}
