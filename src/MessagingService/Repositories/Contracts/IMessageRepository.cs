using System.Collections.Generic;
using System.Threading.Tasks;
using MessagingService.Models;

namespace MessagingService.Repositories.Contracts
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync(MessageSpecification specification);

        Task<Message> AddMessageAsync(SendMessageParameters parameters);

        Task<IEnumerable<User>> GetUserAsync();
    }
}