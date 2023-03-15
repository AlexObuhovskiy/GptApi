using GtpApi.Dto.Gpt;
using GtpApi.Dto.Gpt.ChatCompletions;
using System.Text.Json.Serialization;

namespace GtpApi.Dto;

public class ChatCompletionRequestDto
{
    [JsonIgnore]
    public string? Model { get; set; }
    public string Question { get; set; }
    public string? SetupMessage { get; set; }

    public GptChatCompletionRequestDto ToGptChatCompletionRequestDto()
    {
        GptChatCompletionRequestDto requestDto = new GptChatCompletionRequestDto();

        requestDto.Model = Model;
        requestDto.Messages = new List<GptMessageDto>
        {
            new GptMessageDto
            {
                Role = "user",
                Content = Question
            }
        };

        if (!string.IsNullOrWhiteSpace(SetupMessage))
        {
            requestDto.Messages.Add(new GptMessageDto
            {
                Role = "system",
                Content = SetupMessage
            });
        }

        return requestDto;
    }
}
