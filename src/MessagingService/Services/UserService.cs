using System.Collections.Generic;
using System.Threading.Tasks;
using MessagingService.Models;
using MessagingService.Services.Contracts;

namespace MessagingService.Services
{
    public class UserService : IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return Task.FromResult<IEnumerable<User>>(new []
            {
                new User{ Name = "user1"},
                new User{ Name = "user2"},
                new User{ Name = "user3"},
            });
        }
    }
}