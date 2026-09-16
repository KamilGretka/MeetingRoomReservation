using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private static readonly string value = "Service is running";
        
        [HttpGet]
        public IActionResult GetHealthStatus()
        {
            return Ok(value);
        }
    }
}
