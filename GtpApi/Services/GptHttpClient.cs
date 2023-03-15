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
    Task<GptCompletionResponseDto> GenerateChatCompletionAsync(GptChatCompletionRequestDto requestDto);
}

public class GptHttpClient : IGptHttpClient
{
    private readonly Uri _openAiGptUrl = new("https://api.openai.com/v1/");
    private const string Completions = "completions";
    private const string Chat = "chat";
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

        var completionsUri = new Uri(_openAiGptUrl, Completions);
        var gptResponseDto = await SendMessageAsync(completionsUri, request);
               
        string responseText = gptResponseDto.ResponseObject.choices[0].text;

        var gptCompletionResponseDto = new GptCompletionResponseDto
        {
            ChatRequest = requestDto.Question,
            ChatResponse = responseText.TrimStart('\n', '\t', '\r'),
            ElapsedMilliseconds = gptResponseDto.Stopwatch.ElapsedMilliseconds,
            Model = requestDto.Model,
            MaxTokens = requestDto.MaxTokens,
            Temperature = requestDto.Temperature,
            RequestDateTime = DateTime.UtcNow,
            QuestionTokenAmount = gptResponseDto.ResponseObject.usage.prompt_tokens,
            ResponseTokenAmount = gptResponseDto.ResponseObject.usage.completion_tokens,
            TotalTokenAmount = gptResponseDto.ResponseObject.usage.total_tokens,
        };

        return gptCompletionResponseDto;
    }

    public async Task<GptCompletionResponseDto> GenerateChatCompletionAsync(GptChatCompletionRequestDto requestDto)
    {
        var request = new
        {
            model = requestDto.Model,
            messages = new[] {
                new 
                {
                    role = "system",
                    content = requestDto.SetupMessage
                },
                new
                {
                    role = "user",
                    content = requestDto.Question
                },
            }
        };

        var completionsUri = new Uri(_openAiGptUrl, $"{Chat}/{Completions}");
        var gptResponseDto = await SendMessageAsync(completionsUri, request);        

        string responseText = gptResponseDto.ResponseObject.choices[0].message.content;

        var gptCompletionResponseDto = new GptCompletionResponseDto
        {
            ChatRequest = requestDto.Question,
            ChatSetupMessage = requestDto.SetupMessage,
            ChatResponse = responseText.TrimStart('\n', '\t', '\r'),
            ElapsedMilliseconds = gptResponseDto.Stopwatch.ElapsedMilliseconds,
            Model = requestDto.Model,
            MaxTokens = requestDto.MaxTokens,
            Temperature = requestDto.Temperature,
            RequestDateTime = DateTime.UtcNow,
            QuestionTokenAmount = gptResponseDto.ResponseObject.usage.prompt_tokens,
            ResponseTokenAmount = gptResponseDto.ResponseObject.usage.completion_tokens,
            TotalTokenAmount = gptResponseDto.ResponseObject.usage.total_tokens,
        };

        return gptCompletionResponseDto;
    }

    private async Task<GptResponseDto> SendMessageAsync(Uri uri, object request)
    {
        var result = new GptResponseDto();
        var requestJson = JsonConvert.SerializeObject(request);
        var content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json);

        result.Stopwatch = new Stopwatch();
        result.Stopwatch.Start();
        var response = await _httpClient.PostAsync(uri, content);
        result.Stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to generate text from OpenAI");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
        result.ResponseObject = responseObject!;

        return result;
    }
}
