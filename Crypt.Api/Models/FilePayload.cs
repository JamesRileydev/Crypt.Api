using System;

namespace Crypt.Api.Models
{
    public class FilePayload
    {
        public Memory<byte> CipherText { get; set; }

        public Memory<byte> FileContent { get; set; }

        public Memory<byte> Nonce { get; set; }

        public Memory<byte> Tag { get; set; }
    }
}
