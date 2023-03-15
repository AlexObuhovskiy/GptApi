using System.Text.Json.Serialization;

namespace GtpApi.Dto.Gpt
{
    public class GptChoiceDto
    {
        [JsonPropertyName("message")]
        public GptMessageDto Message { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
