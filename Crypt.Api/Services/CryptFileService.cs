using Crypt.Api.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Crypt.Api.Services
{
    public interface ICryptFileService 
    {
        (Memory<byte>, ServiceError) ProcessFile(CryptRequest request);    
    }

    public class CryptFileService : ICryptFileService
    {
        private const string Encrypt = "encrypt";
        private const string Decrypt = "decrypt";

        private ILogger<CryptFileService> Log;

        public CryptFileService(ILogger<CryptFileService> log)
        {
            Log = log;
        }

        public (Memory<byte>, ServiceError) ProcessFile(CryptRequest request) => request switch
        {
            { Method: Encrypt } => EncryptFile(new Memory<byte>()),
            { Method: Decrypt} => DecryptFile(new Memory<byte>()),

            _ => throw new NotImplementedException()
        };

        private (Memory<byte>, ServiceError) DecryptFile(Memory<byte> memory)
        {
            Log.LogError("Decrypt file functionality not yet implemented");

            return (null, new ServiceError
            {
                Exception = new NotImplementedException("Decrypt file functionality not yet implemented")
            });
        }

        private (Memory<byte>, ServiceError) EncryptFile(Memory<byte> file)
        {
            Log.LogError("Encrypt file functionality not yet implemented");

            return (null, new ServiceError 
            { 
                Exception = new NotImplementedException("Encrypt file functionality not yet implemented")            
            });
        }
    }
}
