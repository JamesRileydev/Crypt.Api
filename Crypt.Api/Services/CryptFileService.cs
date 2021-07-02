using Crypt.Api.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crypt.Api.Services
{
    public interface ICryptFileService
    {
        ValueTask<(Memory<byte>, ServiceError)> ProcessFile(CryptRequest request);
    }

    public class CryptFileService : ICryptFileService
    {
        private const string Encrypt = "encrypt";
        private const string Decrypt = "decrypt";

        private IHttpClientFactory HttpClient { get; }

        private ILogger<CryptFileService> Log;

        public CryptFileService(ILogger<CryptFileService> log, IHttpClientFactory httpClient)
        {
            Log = log;
            HttpClient = httpClient;
        }

        public async ValueTask<(Memory<byte>, ServiceError)> ProcessFile(CryptRequest request) => request switch
        {
            { Method: Encrypt } => await EncryptFile(new Memory<byte>()).ConfigureAwait(false),
            { Method: Decrypt } => await DecryptFile(new Memory<byte>()).ConfigureAwait(false),

            _ => throw new NotImplementedException()
        };

        private async ValueTask<(Memory<byte>, ServiceError)> DecryptFile(Memory<byte> memory)
        {
            Log.LogError("Decrypt file functionality not yet implemented");

            await Task.CompletedTask.ConfigureAwait(false);

            return (null, new ServiceError
            {
                Exception = new NotImplementedException("Decrypt file functionality not yet implemented")
            });
        }

        private async ValueTask<(Memory<byte>, ServiceError)> EncryptFile(Memory<byte> file)
        {
            Log.LogError("Encrypt file functionality not yet implemented");

            var (randomBytes, err) = GetRandomBytes().GetAwaiter().GetResult();

            if (err != null)
            {
                return (null, err);
            }

            var (x, y, z) = await GetKeyAndNonce(randomBytes).ConfigureAwait(false);

            return (null, null);
        }

        private async ValueTask<(string, ServiceError)> GetRandomBytes()
        {
            //Set up HttpClient here
            var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.10:8200/v1/sys/tools/random/44");
            request.Headers.Add("X-Vault-Token", "s.COJdph5fLeTQZ8E1vW6TsiXq");

            var client = HttpClient.CreateClient();
            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                return (null, new ServiceError
                {
                    Exception = ex
                });
            }

            var contents = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var dataObject = JObject.Parse(contents);

            var randomByteString = dataObject["data"]?["random_bytes"]?.Value<string>();

            return (randomByteString, null);
        }

        private async ValueTask<(Memory<byte>, Memory<byte>, ServiceError)> GetKeyAndNonce(string randomBytes)
        {
            var buffer = new Memory<byte>(Encoding.UTF8.GetBytes(randomBytes));

            var response = Base64.DecodeFromUtf8InPlace(buffer.Span, out var bytesWritten);

            buffer = buffer.Slice(0, bytesWritten);

            var key = buffer.Slice(0, 32);
            var nonce = buffer.Slice(32, 12);

            await Task.CompletedTask.ConfigureAwait(false);

            return (null, null, null);
        }
    }
}
