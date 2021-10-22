using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MessagingService.Models;
using StackExchange.Redis;

namespace MessagingService.Data.Services
{
    public static class RedisStreamMessageConverter
    {
        private const string UserNameFieldName = "UserName";
        private const string CreatedDateTimeFieldName = "CreatedDateTime";
        private const string ContentFieldName = "Content";

        public static NameValueEntry[] ConvertToNameValueEntries(this Message message)
        {
            return new[]
            {
                new NameValueEntry(UserNameFieldName, message.UserName),
                new NameValueEntry(CreatedDateTimeFieldName,
                    Convert.ToString(message.CreatedDateTime, CultureInfo.InvariantCulture)),
                new NameValueEntry(ContentFieldName, message.Content),
            };
        }

        public static Message ConvertToMessage(string messageId, NameValueEntry[] values)
        {
            var valuesDict = values.ToDictionary(value => value.Name.ToString(), value => value.Value.ToString());
            return new Message
            {
                Id = messageId,
                UserName = valuesDict.ContainsKey(UserNameFieldName) ? valuesDict[UserNameFieldName] : default,
                CreatedDateTime = valuesDict.ContainsKey(CreatedDateTimeFieldName)
                    ? Convert.ToDateTime(valuesDict[CreatedDateTimeFieldName])
                    : default,
                Content = valuesDict.ContainsKey(ContentFieldName) ? valuesDict[ContentFieldName] : default,
            };
        }

        public static IEnumerable<Message> ConvertToMessages(this IEnumerable<StreamEntry> streamEntries)
        {
            var messagesDict = streamEntries.ToDictionary(
                message => message.Id.ToString(),
                message => message.Values);
            return messagesDict.Keys.Select(messageId => ConvertToMessage(messageId, messagesDict[messageId]));
        }
    }
}