using System.Diagnostics;

namespace GtpApi.Dto;

public class GptResponseDto
{
    public dynamic ResponseObject { get; set; }
    public Stopwatch Stopwatch { get; set; }
}
