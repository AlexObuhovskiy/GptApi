using System.Text.Json.Serialization;

namespace GtpApi.Dto;

public class CompletionRequestDto
{
    [JsonIgnore]
    public string? Model { get; set; }

    [JsonIgnore]
    public int MaxTokens { get; set; }

    [JsonIgnore]
    public double Temperature { get; set; }

    public string Question { get; set; }
}
