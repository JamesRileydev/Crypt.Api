using Crypt.Api.Models;
using Crypt.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public async ValueTask<IActionResult> ProcessFile([FromRoute]string method)
        {
            var rawFile = Request.Form?.Files;

            if (rawFile == null)
            {
                Log.LogError("Did not recieve file for {method}", method);

                return BadRequest($"Did not recieve file for {method}");
            }

            var file = rawFile.First();

            using var ms = new MemoryStream();

            var newFile = file.CopyToAsync(ms).ConfigureAwait(false);

            var request = new CryptRequest
            {
                File = new FilePayload
                {
                    FileContent = ms.ToArray()
                },
                Method = method
            };

            var (result, error) = await CryptFileSvc.ProcessFile(request).ConfigureAwait(false);

            if (error != null)
            {
                return StatusCode(500, error.Exception.Message);
            }

            return Ok();
        }
    }
}
