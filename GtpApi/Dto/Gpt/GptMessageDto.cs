using System.Text.Json.Serialization;

namespace GtpApi.Dto.Gpt
{
    public class GptMessageDto
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
