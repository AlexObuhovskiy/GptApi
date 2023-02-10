using GtpApi.Dto;
using GtpApi.Setup;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Mime;
using System.Text;

namespace GtpApi.Services;

public interface IGptHttpClient
{
    Task<string> GetGptModels();
    Task<GptCompletionResponseDto> GenerateCompletionAsync(GptCompletionRequestDto requestDto);
}

public class GptHttpClient : IGptHttpClient
{
    private readonly Uri _openAiGptUrl = new("https://api.openai.com/v1/completions");
    private const string Completions = "completions";
    private const string Models = "models";

    private readonly HttpClient _httpClient;

    public GptHttpClient(IOptions<OpenApiOptions> gptOptions)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", gptOptions.Value.ApiKey);
    }

    public async Task<string> GetGptModels()
    {
        var modelsUri = new Uri(_openAiGptUrl, Models);
        var response = await _httpClient.GetAsync(modelsUri);

        var responseJson = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseJson);

        return responseJson;
    }

    public async Task<GptCompletionResponseDto> GenerateCompletionAsync(GptCompletionRequestDto requestDto)
    {
        var request = new
        {
            model = requestDto.Model,
            prompt = requestDto.Question,
            max_tokens = requestDto.MaxTokens,
            temperature = requestDto.Temperature
        };

        var requestJson = JsonConvert.SerializeObject(request);
        var content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json);
        var completionsUri = new Uri(_openAiGptUrl, Completions);

        Stopwatch a = new Stopwatch();
        a.Start();
        var response = await _httpClient.PostAsync(completionsUri, content);
        a.Stop();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to generate text from OpenAI");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
        string responseText = responseObject!.choices[0].text;


        var gptCompletionResponseDto = new GptCompletionResponseDto
        {
            ChatRequest = requestDto.Question,
            ChatResponse = responseText.TrimStart('\n', '\t', '\r'),
            ElapsedMilliseconds = a.ElapsedMilliseconds,
            Model = requestDto.Model,
            MaxTokens = requestDto.MaxTokens,
            Temperature = requestDto.Temperature,
            RequestDateTime = DateTime.UtcNow,
            QuestionTokenAmount = responseObject.usage.prompt_tokens,
            ResponseTokenAmount = responseObject.usage.completion_tokens,
            TotalTokenAmount = responseObject.usage.total_tokens,
        };

        return gptCompletionResponseDto;
    }
}
