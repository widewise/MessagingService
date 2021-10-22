using System.Collections.Generic;
using System.Threading.Tasks;
using MessagingService.Models;

namespace MessagingService.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
    }
}