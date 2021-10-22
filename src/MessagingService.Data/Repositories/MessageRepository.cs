using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingService.Data.Services;
using MessagingService.Extensions;
using MessagingService.Models;
using MessagingService.Repositories.Contracts;
using StackExchange.Redis;

namespace MessagingService.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private const string MessagesStreamName = "messages";
        private static readonly ConcurrentBag<Message> Messages = new(new[]
        {
            new Message { Id = "1", UserName = "user1", Content = "some message" },
            new Message { Id = "2", UserName = "user2", Content = "some message" },
            new Message { Id = "3", UserName = "user3", Content = "some message" },
        });

        private readonly IDatabase _database;
        
        public MessageRepository(
            IDatabase database)
        {
            _database = Preconditions.CheckNotNull(database, nameof(database));
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(MessageSpecification specification)
        {
            Preconditions.CheckNotNull(specification, nameof(specification));

            var messages = !string.IsNullOrEmpty(specification.FromMessageId)
                ? await _database.StreamRangeAsync(
                    MessagesStreamName,
                    minId: specification.FromMessageId)
                : await _database.StreamRangeAsync(
                    MessagesStreamName,
                    count: specification.Limit);

            return messages.ConvertToMessages();
        }

        public async Task<Message> AddMessageAsync(SendMessageParameters parameters)
        {
            Preconditions.CheckNotNull(parameters, nameof(parameters));

            var message = new Message
            {
                UserName = parameters.UserName,
                CreatedDateTime = DateTime.UtcNow,
                Content = parameters.Content
            };

            message.Id = await _database.StreamAddAsync(
                MessagesStreamName,
                message.ConvertToNameValueEntries());

            return message;
        }

        public Task<IEnumerable<User>> GetUserAsync()
        {
            var users = Messages
                .Select(message => new User
                {
                    Name = message.UserName
                })
                .Distinct()
                .ToArray();

            return Task.FromResult<IEnumerable<User>>(users);
        }
    }
}