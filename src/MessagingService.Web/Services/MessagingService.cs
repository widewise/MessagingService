using System;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MessagingService.Extensions;
using MessagingService.Models;
using MessagingService.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace MessagingService.Web.Services
{
    public class MessagingService: Messaging.MessagingBase
    {
        private readonly ILogger<MessagingService> _logger;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessagingService(
            ILogger<MessagingService> logger,
            IMapper mapper,
            IMessageService messageService,
            IUserService userService)
        {
            _logger = Preconditions.CheckNotNull(logger, nameof(logger));
            _mapper = Preconditions.CheckNotNull(mapper, nameof(mapper));
            _messageService = Preconditions.CheckNotNull(messageService, nameof(messageService));
            _userService = Preconditions.CheckNotNull(userService, nameof(userService));
        }

        public override async Task<MessagesResponse> GetMessages(
            MessagesRequest request,
            ServerCallContext context)
        {
            var specification = _mapper.Map<MessageSpecification>(request);

            var messages = await _messageService.GetMessagesAsync(specification);

            return _mapper.Map<MessagesResponse>(messages);
        }

        public override async Task GetMessagesStreamed(
            EmptyRequest request,
            IServerStreamWriter<MessageResponse> responseStream,
            ServerCallContext context)
        {
            await foreach (var message in _messageService.GetMessagesStreamAsync(context.CancellationToken))
            {
                await responseStream.WriteAsync(_mapper.Map<MessageResponse>(message));
            }

            await Task.CompletedTask;
        }

        public override async Task<MessageResponse> SendMessage(
            MessageRequest request,
            ServerCallContext context)
        {
            try
            {
                var parameters = _mapper.Map<MessageRequest, SendMessageParameters>(request);

                var message = await _messageService.SendMessageAsync(parameters);
                var response = _mapper.Map<Message, MessageResponse>(message);

                return response;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Sending message {Content} from user {User} error",
                    request.Content,
                    request.UserName);
                throw;
            }
        }

        public override async Task<UsersResponse> GetUsers(
            EmptyRequest request,
            ServerCallContext context)
        {
            var users = await _userService.GetUsersAsync();
            return _mapper.Map<UsersResponse>(users);
        }
    }
}