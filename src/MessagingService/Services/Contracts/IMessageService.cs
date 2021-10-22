using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MessagingService.Models;

namespace MessagingService.Services.Contracts
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesAsync(MessageSpecification specification);

        IAsyncEnumerable<Message> GetMessagesStreamAsync(CancellationToken cancellationToken);

        Task<Message> SendMessageAsync(SendMessageParameters parameters);
    }
}