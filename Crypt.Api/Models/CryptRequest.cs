using Microsoft.AspNetCore.Http;

namespace Crypt.Api.Models
{
    public class CryptRequest
    {
        public string Method { get; set; }

        public FilePayload File { get; set; }
    }
}