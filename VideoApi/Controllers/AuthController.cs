using Microsoft.AspNetCore.Mvc;

namespace VideoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<string> RegisterAsync()
        {
            await Task.Delay(200);

            return "REGISTER";
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync()
        {
            await Task.Delay(200);

            return "LOGIN";
        }
    }
}