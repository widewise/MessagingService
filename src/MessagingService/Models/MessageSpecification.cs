using System.Diagnostics.CodeAnalysis;

namespace MessagingService.Models
{
    [ExcludeFromCodeCoverage]
    public record MessageSpecification(
        int Limit,
        string FromMessageId = null);
}