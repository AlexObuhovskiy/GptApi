using GtpApi.DataModel.DataAccess;
using GtpApi.DataModel.Entities;
using GtpApi.Dto;

namespace GtpApi.Services
{
    public interface IGptService
    {
        Task<string> GetGptModels();
        Task<GptCompletionResponseDto> GenerateCompletionAsync(GenerateCompletionRequestDto requestDto);
        Task<GptCompletionResponseDto> GenerateChatCompletion(GenerateChatCompletionRequestDto requestDto);
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

        public async Task<GptCompletionResponseDto> GenerateCompletionAsync(GenerateCompletionRequestDto requestDto)
        {
            var request = new GptCompletionRequestDto
            {
                //Model = "text-ada-001",
                Model = "text-davinci-003",
                //Model = "gpt-3.5-turbo",
                Question = requestDto.Question,
                MaxTokens = 100,
                Temperature = 0.5
            };

            var result = await _gptHttpClient.GenerateCompletionAsync(request);
            var chatInfo = new ChatInfo
            {
                ChatRequest = result.ChatRequest,
                ChatResponse = result.ChatResponse.Trim('\n', '\t', '\r', ' '),
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

        public async Task<GptCompletionResponseDto> GenerateChatCompletion(GenerateChatCompletionRequestDto requestDto)
        {
            var request = new GptChatCompletionRequestDto
            {
                Model = "gpt-3.5-turbo",
                MaxTokens = 100,
                Temperature = 0.5,
                Question = requestDto.Question,
                SetupMessage = requestDto.SetupMessage
            };

            var result = await _gptHttpClient.GenerateChatCompletionAsync(request);
            var chatInfo = new ChatInfo
            {
                ChatRequest = result.ChatRequest,
                ChatSetupMessage = result.ChatSetupMessage,
                ChatResponse = result.ChatResponse.Trim('\n', '\t', '\r', ' '),
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
    }
}
