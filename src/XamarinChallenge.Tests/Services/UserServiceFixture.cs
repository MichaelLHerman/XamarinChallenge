using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Xamarin.Essentials.Interfaces;
using XamarinChallenge.Services;

namespace XamarinChallenge.Tests.Services
{
    [TestClass]
    public class UserServiceFixture
    {
        UserService _subject;
        Mock<ISecureStorage> _mockPreferences;

        [TestInitialize]
        public void Setup()
        {
            _mockPreferences = new Mock<ISecureStorage>();
            _subject = new UserService(_mockPreferences.Object);
        }

        [TestMethod]
        public async Task AddUserAsync_CallsSecureStorageSet()
        {
            _mockPreferences.Setup(s => s.SetAsync(It.IsAny<string>(), It.IsAny<string>()));
            const string UserName = "username";
            const string Password = "password";
            await _subject.AddUserAsync(UserName, Password);

            _mockPreferences.Verify(s => s.SetAsync(It.IsAny<string>(), It.Is<string>(s => s.Contains(UserName) && s.Contains(Password))), Times.Once);
        }

        [TestMethod]
        public async Task GetUserListAsync_CallsSecureStorageSet()
        {
            const string UserName = "username";
            const string Password = "password";

            _mockPreferences.Setup(s => s.GetAsync(It.IsAny<string>())).ReturnsAsync(JsonConvert.SerializeObject(new List<User>
            {
                new User(UserName, Password)
            }));

            var list = await _subject.GetUserListAsync();

            _mockPreferences.Verify(s => s.GetAsync(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(UserName, list.First().UserName);
            Assert.AreEqual(Password, list.First().Password);
        }
    }
}
