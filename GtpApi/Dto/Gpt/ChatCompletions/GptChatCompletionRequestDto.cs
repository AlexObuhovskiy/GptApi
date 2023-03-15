using System.Text.Json.Serialization;

namespace GtpApi.Dto.Gpt.ChatCompletions
{
    public class GptChatCompletionRequestDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<GptMessageDto> Messages { get; set; }
    }
}
