using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using MessagingService.Web;

namespace MessagingService.Client
{
    class Program
    {
        private const string ServiceUrl = "http://localhost:5000";
        private const int MessagesCountToMatch = 20;
        private static bool _exit;

        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress(ServiceUrl);
            var client = new Messaging.MessagingClient(channel);

            while (!_exit)
            {
                Console.WriteLine("Enter command:");
                var command = Console.ReadLine();
                ProcessCommand(command, client);
            }

            Console.WriteLine("Admin application closed");
        }

        static void ProcessCommand(
            string command,
            Messaging.MessagingClient client)
        {
            switch (command)
            {
                case "stop":
                    _exit = true;
                    Console.WriteLine("Messaging process stopped");
                    break;

                case "ls":
                    LsCommandProcessor(client);
                    break;

                case "watch":
                    WatchCommandProcessor(client);
                    break;

                case "streamed-watch":
                    StreamedWatchCommandProcessor(client).Wait();
                    break;

                case "send":
                    SendCommandProcessor(client);
                    break;
            }
        }

        private static void LsCommandProcessor(Messaging.MessagingClient client)
        {
            var users = client.GetUsers(new EmptyRequest());
            if (users?.Users == null
            )
            {
                Console.WriteLine("There are no connected users");
                return;
            }

            var outputUsers = users.Users.Where(user => !string.IsNullOrWhiteSpace(user.UserName)).ToArray();
            if (outputUsers.Length == 0)
            {
                Console.WriteLine("There are no connected users");
                return;
            }

            foreach (var userResponse in outputUsers)
            {
                Console.WriteLine(userResponse.UserName);
            }
        }

        private static void WatchCommandProcessor(Messaging.MessagingClient client)
        {
            var messages = client.GetMessages(new MessagesRequest
            {
                Limit = MessagesCountToMatch
            });
            if (messages?.Messages == null || messages.Messages.Count == 0)
            {
                Console.WriteLine("You don't have messages");
                return;
            }

            foreach (var messageResponse in messages.Messages)
            {
                Console.WriteLine($"{messageResponse.UserName}: {messageResponse.Content}");
            }
        }

        private static void SendCommandProcessor(Messaging.MessagingClient client)
        {
            Console.Write("Please enter user name:");
            var userName = Console.ReadLine();

            Console.Write("Please enter message:");
            var content = Console.ReadLine();

            var message = client.SendMessage(new MessageRequest
            {
                UserName = userName,
                Content = content
            });

            Console.WriteLine($"Message with id {message.Id} was sent");
        }

        private static async Task StreamedWatchCommandProcessor(Messaging.MessagingClient client)
        {
            var asyncServerStreamingCall = client.GetMessagesStreamed(new EmptyRequest());

            while (await asyncServerStreamingCall.ResponseStream.MoveNext(CancellationToken.None))
            {
                var messageResponse = asyncServerStreamingCall.ResponseStream.Current;
                Console.WriteLine($"{messageResponse.UserName}: {messageResponse.Content}");
            }
        }
    }
}