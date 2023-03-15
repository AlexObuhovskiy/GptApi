namespace GtpApi.Dto;

public class GptChatCompletionRequestDto
{
    public string Model { get; set; }
    public string Question { get; set; }
    public string SetupMessage { get; set; }
    public int MaxTokens { get; set; }
    public double Temperature { get; set; }
}
