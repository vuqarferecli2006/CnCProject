using System.Text.Json.Serialization;

namespace CnC.Application.DTOs.FaceBook;

public record FacebookPicture
{
    [JsonPropertyName("data")]
    public FacebookPictureData Data { get; set; }= new();
}
