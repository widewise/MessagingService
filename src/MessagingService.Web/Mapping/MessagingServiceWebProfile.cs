using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MessagingService.Models;

namespace MessagingService.Web.Mapping
{
    [ExcludeFromCodeCoverage]
    public class MessagingServiceWebProfile : Profile
    {
        public MessagingServiceWebProfile()
        {
            CreateMap<MessagesRequest, MessageSpecification>();
            CreateMap<MessageRequest, SendMessageParameters>();

            CreateMap<Message, MessageResponse>();
            CreateMap<IEnumerable<Message>, MessagesResponse>()
                .ForMember(destination => destination.Messages,
                    options => options.MapFrom(source => source));

            CreateMap<User, UserResponse>()
                .ForMember(destination => destination.UserName,
                    options => options.MapFrom(source => source.Name));
            CreateMap<IEnumerable<User>, UsersResponse>()
                .ForMember(destination => destination.Users,
                    options => options.MapFrom(source => source));
        }
    }
}