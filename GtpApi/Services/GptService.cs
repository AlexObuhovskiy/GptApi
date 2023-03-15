using GtpApi.DataModel.DataAccess;
using GtpApi.DataModel.Entities;
using GtpApi.Dto;

namespace GtpApi.Services
{
    public interface IGptService
    {
        Task<string> GetGptModels();
        Task<CompletionResponseDto> GenerateCompletionAsync(CompletionRequestDto requestDto);
        Task<CompletionResponseDto> GenerateChatCompletion(ChatCompletionRequestDto requestDto);
    }

    public class GptService : IGptService
    {
        private readonly IGptHttpClient _gptHttpClient;
        private readonly DataContext _db;

        public GptService(
            IGptHttpClient gptHttpClient,
            DataContext db)
        {
            _gptHttpClient = gptHttpClient;
            _db = db;
        }

        public Task<string> GetGptModels()
        {
            return _gptHttpClient.GetGptModels();
        }

        public async Task<CompletionResponseDto> GenerateCompletionAsync(CompletionRequestDto requestDto)
        {
            //Model = "text-ada-001",
            requestDto.Model = "text-davinci-003";
            requestDto.MaxTokens = 100;
            requestDto.Temperature = 0.5;

            var result = await _gptHttpClient.GenerateCompletionAsync(requestDto);
            var chatInfo = new ChatInfo
            {
                ChatRequest = result.ChatRequest,
                ChatResponse = result.ChatResponse,
                ElapsedMilliseconds = result.ElapsedMilliseconds,
                Model = result.Model,
                MaxTokens = result.MaxTokens,
                Temperature = result.Temperature,
                RequestDateTime = result.RequestDateTime,
                QuestionTokenAmount = result.QuestionTokenAmount,
                ResponseTokenAmount = result.ResponseTokenAmount,
                TotalTokenAmount = result.TotalTokenAmount,
            };

            _db.ChatInfos.Add(chatInfo);
            await _db.SaveChangesAsync();
            result.Id = chatInfo.Id;

            return result;
        }

        public async Task<CompletionResponseDto> GenerateChatCompletion(ChatCompletionRequestDto requestDto)
        {
            //Model = "gpt-3.5-turbo",
            requestDto.Model = "gpt-3.5-turbo-0301";

            var result = await _gptHttpClient.GenerateChatCompletionAsync(requestDto);
            var chatInfo = new ChatInfo
            {
                ChatRequest = result.ChatRequest,
                ChatSetupMessage = result.ChatSetupMessage,
                ChatResponse = result.ChatResponse,
                ElapsedMilliseconds = result.ElapsedMilliseconds,
                Model = result.Model,
                RequestDateTime = result.RequestDateTime,
                QuestionTokenAmount = result.QuestionTokenAmount,
                ResponseTokenAmount = result.ResponseTokenAmount,
                TotalTokenAmount = result.TotalTokenAmount,
            };

            _db.ChatInfos.Add(chatInfo);
            await _db.SaveChangesAsync();
            result.Id = chatInfo.Id;

            return result;
        }
    }
}
