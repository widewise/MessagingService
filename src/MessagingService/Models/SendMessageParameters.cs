using System.Diagnostics.CodeAnalysis;

namespace MessagingService.Models
{
    [ExcludeFromCodeCoverage]
    public record SendMessageParameters(
        string UserName,
        string Content);
}