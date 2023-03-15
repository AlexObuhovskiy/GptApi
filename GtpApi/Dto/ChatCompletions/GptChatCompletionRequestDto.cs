namespace GtpApi.Dto.ChatCompletions
{
    public class GptChatCompletionRequestDto
    {
        public string model { get; set; }
        public List<GptMessageDto> messages { get; set; }
    }
}
