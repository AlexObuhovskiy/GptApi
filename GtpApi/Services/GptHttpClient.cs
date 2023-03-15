using GtpApi.Dto;
using GtpApi.Dto.Gpt.ChatCompletions;
using GtpApi.Dto.Gpt.Completions;
using GtpApi.Setup;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace GtpApi.Services;

public interface IGptHttpClient
{
    Task<string> GetGptModels();
    Task<CompletionResponseDto> GenerateCompletionAsync(CompletionRequestDto requestDto);
    Task<CompletionResponseDto> GenerateChatCompletionAsync(ChatCompletionRequestDto requestDto);
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

    public async Task<CompletionResponseDto> GenerateCompletionAsync(CompletionRequestDto requestDto)
    {
        var request = new GptCompletionRequestDto
        {
            Model = requestDto.Model!,
            Prompt = requestDto.Question,
            MaxTokens = requestDto.MaxTokens,
            Temperature = requestDto.Temperature
        };

        var completionsUri = new Uri(_openAiGptUrl, Completions);
        var responseDto = await SendMessageAsync(completionsUri, request);

        var gptCompletionResponse = JsonSerializer.Deserialize<GptCompletionResponseDto>(responseDto.ResponseJson);
        if (gptCompletionResponse == null)
        {
            throw new Exception($"Can't deserialize: {responseDto.ResponseJson}.");
        }

        var gptCompletionResponseDto = new CompletionResponseDto
        {
            ChatRequest = requestDto.Question,
            ElapsedMilliseconds = responseDto.Stopwatch.ElapsedMilliseconds,
            Model = requestDto.Model!,
            MaxTokens = requestDto.MaxTokens,
            Temperature = requestDto.Temperature,
            RequestDateTime = DateTime.UtcNow,
            QuestionTokenAmount = gptCompletionResponse.Usage.PromptTokens,
            ResponseTokenAmount = gptCompletionResponse.Usage.CompletionTokens,
            TotalTokenAmount = gptCompletionResponse.Usage.TotalTokens,
            ChatResponse = gptCompletionResponse.Choices.First().Text.TrimStart('\n', '\t', '\r')
        };

        return gptCompletionResponseDto;
    }

    public async Task<CompletionResponseDto> GenerateChatCompletionAsync(ChatCompletionRequestDto requestDto)
    {
        var request = requestDto.ToGptChatCompletionRequestDto();

        var completionsUri = new Uri(_openAiGptUrl, $"{Chat}/{Completions}");
        var responseDto = await SendMessageAsync(completionsUri, request);

        var gptChatCompletionResponse = JsonSerializer.Deserialize<GptChatCompletionResponseDto>(responseDto.ResponseJson);
        if (gptChatCompletionResponse == null)
        {
            throw new Exception($"Can't deserialize: {responseDto.ResponseJson}.");
        }

        var gptCompletionResponseDto = new CompletionResponseDto
        {
            ChatRequest = requestDto.Question,
            ElapsedMilliseconds = responseDto.Stopwatch.ElapsedMilliseconds,
            Model = requestDto.Model!,
            RequestDateTime = DateTime.UtcNow,
            QuestionTokenAmount = gptChatCompletionResponse.Usage.PromptTokens,
            ResponseTokenAmount = gptChatCompletionResponse.Usage.CompletionTokens,
            TotalTokenAmount = gptChatCompletionResponse.Usage.TotalTokens,
            ChatResponse = gptChatCompletionResponse.Choices.First().Message.Content.TrimStart('\n', '\t', '\r')
        };

        return gptCompletionResponseDto;
    }

    private async Task<ClientResponseDto> SendMessageAsync(Uri uri, object request)
    {
        var result = new ClientResponseDto();

        var requestJson = JsonSerializer.Serialize(request);

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
        result.ResponseJson = responseJson;

        return result;
    }
}
