using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MessagingService.Extensions;
using MessagingService.Models;
using MessagingService.Repositories.Contracts;
using MessagingService.Services.Contracts;

namespace MessagingService.Services
{
    public class MessageService : IMessageService
    {
        private const int TimeoutOfCheckingMessageForStream = 500;
        private const int MessagesCountToRecheckForStream = 10;
        private readonly IMessageRepository _repository;

        public MessageService(
            IMessageRepository repository)
        {
            _repository = Preconditions.CheckNotNull(repository, nameof(repository));
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(MessageSpecification specification)
        {
            Preconditions.CheckNotNull(specification, nameof(specification));

            return await _repository.GetMessagesAsync(specification);
        }

        public async IAsyncEnumerable<Message> GetMessagesStreamAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var fromMessageId = string.Empty;
            while (!cancellationToken.IsCancellationRequested)
            {
                var specification = new MessageSpecification(MessagesCountToRecheckForStream, fromMessageId);

                var messages = await _repository.GetMessagesAsync(specification);
                if (messages != null)
                {
                    messages = messages.Where(message => message.Id != fromMessageId);
                    var lastMessage = messages.LastOrDefault();
                    if (lastMessage != null)
                    {
                        foreach (var message in messages)
                        {
                            yield return message;
                        }
                        fromMessageId = lastMessage.Id;
                    }
                }

                await Task.Delay(TimeoutOfCheckingMessageForStream, cancellationToken);
            }
        }

        public async Task<Message> SendMessageAsync(SendMessageParameters parameters)
        {
            Preconditions.CheckNotNull(parameters, nameof(parameters));

            return await _repository.AddMessageAsync(parameters);
        }
    }
}