using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Rakenne.TestHarness.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApiSettings _apiSettings;

        public ValuesController(IOptionsSnapshot<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_apiSettings);
        }
    }
}
