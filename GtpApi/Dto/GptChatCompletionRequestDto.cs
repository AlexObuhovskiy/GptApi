namespace GtpApi.Dto;

public class GptChatCompletionRequestDto : GptCompletionRequestDto
{
    public string? SetupMessage { get; set; }
}
