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

        [HttpPost]
        public async Task<IActionResult> GenerateTextAsync(string prompt)
        {
            var result = await _gptService.GenerateCompletionAsync(prompt);

            return Ok(result);
        }
    }
}