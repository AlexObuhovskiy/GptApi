using System.Text.Json.Serialization;

namespace GtpApi.Dto.Gpt
{
    public class GptCompletionChoiceDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }
}
