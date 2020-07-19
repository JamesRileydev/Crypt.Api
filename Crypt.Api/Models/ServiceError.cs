using System;

namespace Crypt.Api.Models
{
    public class ServiceError
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
