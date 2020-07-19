using Crypt.Api.Models;
using Crypt.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Crypt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptController : ControllerBase
    {
        private ICryptFileService CryptFileSvc { get; }

        private ILogger<CryptController> Log;

        public CryptController(ICryptFileService cryptFileSvc, ILogger<CryptController> log)
        {
            CryptFileSvc = cryptFileSvc;
            Log = log;
        }
        [HttpPost("{file}/{method}")]
        public IActionResult ProcessFile([FromRoute]string method, [FromForm] CryptRequest request)
        {
            if (request.File == null)
            {
                Log.LogError("Did not recieve file for encryption");

                return BadRequest("Did not recieve file for encryption");
            }

            Log.LogInformation($"Recieved {request.File.FileName} for encryption");
         
            request.Method = method;

            var (result, error) = CryptFileSvc.ProcessFile(request);

            if (error != null)
            {
                return StatusCode(500, error.Exception.Message);
            }

            return Ok();
        }
    }
}
