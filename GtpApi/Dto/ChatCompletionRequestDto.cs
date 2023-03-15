using GtpApi.Dto.ChatCompletions;

namespace GtpApi.Dto;

public class ChatCompletionRequestDto : CompletionRequestDto
{
    public string? SetupMessage { get; set; }

    public GptChatCompletionRequestDto ToGptChatCompletionRequestDto()
    {
        GptChatCompletionRequestDto requestDto = new GptChatCompletionRequestDto();

        requestDto.model = this.Model;
        requestDto.messages = new List<GptMessageDto>
        {
            new GptMessageDto
            {
                role = "user",
                content = this.Question
            }
        };

        if (!string.IsNullOrWhiteSpace(this.SetupMessage))
        {
            requestDto.messages.Add(new GptMessageDto
            {
                role = "system",
                content = this.SetupMessage
            });
        }

        return requestDto;
    }
}
