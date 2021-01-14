using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials.Interfaces;

namespace XamarinChallenge.Services
{
    public class UserService : IUserService
    {
        readonly ISecureStorage _secureStorage;
        const string userSettingsKey = "users";

        public UserService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public async Task AddUserAsync(string userName, string password)
        {
            var newList = (await GetUserListAsync()).Concat(new[]
            {
                new User(userName, password)
            });

            await _secureStorage.SetAsync(userSettingsKey, JsonConvert.SerializeObject(newList));
        }

        public async Task<IEnumerable<User>> GetUserListAsync()
        {
            var settingsString = await _secureStorage.GetAsync(userSettingsKey);

            if (settingsString == null)
            {
                return Enumerable.Empty<User>();
            }
            return JsonConvert.DeserializeObject<List<User>>(settingsString);
        }
    }
}
