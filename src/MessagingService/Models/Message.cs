using System;
using System.Diagnostics.CodeAnalysis;

namespace MessagingService.Models
{
    [ExcludeFromCodeCoverage]
    public class Message
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Content { get; set; }
    }
}