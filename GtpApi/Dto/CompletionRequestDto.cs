namespace GtpApi.Dto;

public class CompletionRequestDto
{
    public string Model { get; set; }
    public string Question { get; set; }
    public int MaxTokens { get; set; }
    public double Temperature { get; set; }
}
