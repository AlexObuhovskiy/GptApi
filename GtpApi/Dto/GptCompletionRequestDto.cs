namespace GtpApi.Dto;

public class GptCompletionRequestDto
{
    public string Model { get; set; }
    public string Question { get; set; }
    public int MaxTokens { get; set; }
    public double Temperature { get; set; }
}
