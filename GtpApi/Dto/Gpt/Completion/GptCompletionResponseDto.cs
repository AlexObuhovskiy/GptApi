using System.Text.Json.Serialization;

namespace GtpApi.Dto.Gpt.Completions
{
    public class GptCompletionResponseDto
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<GptCompletionChoiceDto> Choices { get; set; }

        [JsonPropertyName("usage")]
        public GptUsageDto Usage { get; set; }
    }
}
