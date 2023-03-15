using GtpApi.Dto;
using GtpApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GtpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GptController : ControllerBase
    {
        private readonly IGptService _gptService;
        

        public GptController(IGptService gptService)
        {
            _gptService = gptService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModels()
        {
            var result = await _gptService.GetGptModels();

            return Ok(result);
        }

        [HttpPost("completions")]
        public async Task<IActionResult> GenerateTextAsync([FromBody] CompletionRequestDto requestDto)
        {
            var result = await _gptService.GenerateCompletionAsync(requestDto);

            return Ok(result);
        }

        [HttpPost("chat/completions")]
        public async Task<IActionResult> GenerateTextAsync([FromBody] ChatCompletionRequestDto requestDto)
        {
            var result = await _gptService.GenerateChatCompletion(requestDto);

            return Ok(result);
        }
    }
}