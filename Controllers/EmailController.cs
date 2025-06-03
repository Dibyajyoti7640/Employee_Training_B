using Employee_Training_B.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostmarkDotNet;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var message = new PostmarkMessage()
                {
                    To = request.To,
                    From = "21cse591.dibyajyotisahoo@giet.edu",
                    TrackOpens = true,
                    Subject = request.Subject,
                    TextBody = request.Body
                };
                var client = new PostmarkClient("bd47bbcb-2fe2-4b9b-99d4-d7b95a1149dd");
                var res = await client.SendMessageAsync(message);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
