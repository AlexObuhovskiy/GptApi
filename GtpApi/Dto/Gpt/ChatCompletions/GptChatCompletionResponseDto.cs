using System.Text.Json.Serialization;

namespace GtpApi.Dto.Gpt.ChatCompletions
{
    public class GptChatCompletionResponseDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("usage")]
        public GptUsageDto Usage { get; set; }

        [JsonPropertyName("choices")]
        public List<GptChoiceDto> Choices { get; set; }
    }
}
