using Microsoft.AspNetCore.Mvc;

namespace Test_HealthHub_Api.Controllers
{
    [ApiController]
    [Route("api/mock")]

    public class MockApiController(PublisherState state) : ControllerBase
    {
        [HttpPost("start")]
        public async Task<ActionResult> StartLoop()
        {
            state.Start();
            return Ok();
        }

        [HttpPost("stop")]
        public async Task<ActionResult> StopLoop()
        {
            state.Stop();
            return Ok();
        }
    }
}