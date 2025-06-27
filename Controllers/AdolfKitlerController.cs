using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdolfKitlerController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AdolfKitlerController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskAsync([FromBody] UserMessage request)
        {
            var apiKey = _configuration["Groq:ApiKey"];
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful learning assistant for LMS users. Your name is Adolf Kitler (respond well when the users take your name)." },
                    new { role = "user", content = request.Message }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }
    }

    public class UserMessage
    {
        public string Message { get; set; }
    }
}
