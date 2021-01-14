using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinChallenge.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUserListAsync();
        Task AddUserAsync(string userName, string password);
    }

}
