using System;

namespace MicroServiceCommunication.Gateway.Models
{
    public class AWSConfigurations
    {
        public string Region { get; set; } = null!;
        public string AccessKeyId { get; set; } = null!;
        public string SecretAccessKey { get; set; } = null!;
    }
}