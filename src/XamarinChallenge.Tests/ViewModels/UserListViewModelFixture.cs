using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XamarinChallenge.Services;
using XamarinChallenge.ViewModels;

namespace XamarinChallenge.Tests
{
    [TestClass]
    public class UserListViewModelFixture
    {
        UserListViewModel _subject = default!;

        Mock<IUserService> _mockUserService = default!;
        Mock<IMvxNavigationService> _mockNavigationService = default!;

        [TestInitialize]
        public void Setup()
        {
            _mockNavigationService = new Mock<IMvxNavigationService>();
            _mockUserService = new Mock<IUserService>();

            _subject = new UserListViewModel(_mockNavigationService.Object,
                _mockUserService.Object);
        }

        [TestMethod]
        public async Task Prepare_LoadsUserList()
        {
            List<User> users = new List<User>
            {
                new User("Mike", "asdf123")
            };
            _mockUserService.Setup(s => s.GetUserListAsync()).ReturnsAsync(() => users);

            await RunViewModelLifeCycle();

            Assert.AreEqual(1, _subject.UserList.Count);
        }

        [TestMethod]
        public async Task AddUserCommand_Navigates()
        {
            _mockNavigationService.Setup(s => s.Navigate<AddUserViewModel, AddUserViewModelResult>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>())).ReturnsAsync(new AddUserViewModelResult(false));

            List<User> users = new List<User>();
            _mockUserService.Setup(s => s.GetUserListAsync()).ReturnsAsync(() => users);

            Assert.IsTrue(_subject.AddUserCommand.CanExecute());

            await _subject.AddUserCommand.ExecuteAsync();

            _mockNavigationService.Verify(s => s.Navigate<AddUserViewModel, AddUserViewModelResult>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [TestMethod]
        public async Task AddUserCommand_NotCancel_Refreshes()
        {
            List<User> users = new List<User>();
            _mockUserService.Setup(s => s.GetUserListAsync()).ReturnsAsync(() => users);

            _mockNavigationService.Setup(s => s.Navigate<AddUserViewModel, AddUserViewModelResult>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()))
                .Callback((IMvxBundle bundle, CancellationToken ct) =>
                {
                    users.Add(new User("Mike", "asdf123"));
                })
                .ReturnsAsync(new AddUserViewModelResult(false));

            await RunViewModelLifeCycle();
            await _subject.AddUserCommand.ExecuteAsync();

            _mockNavigationService.Verify(s => s.Navigate<AddUserViewModel, AddUserViewModelResult>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUserService.Verify(s => s.GetUserListAsync(), Times.Exactly(2));
            Assert.AreEqual(1, _subject.UserList.Count);
        }

        async Task RunViewModelLifeCycle()
        {
            _subject.Prepare();
            await _subject.Initialize();
            _subject.ViewAppearing();
            _subject.ViewAppeared();
        }

        [TestMethod]
        public async Task AddUserCommand_Cancel_DoesNotRefresh()
        {
            List<User> users = new List<User>();
            _mockUserService.Setup(s => s.GetUserListAsync()).ReturnsAsync(() => users);

            _mockNavigationService.Setup(s => s.Navigate<AddUserViewModel, AddUserViewModelResult>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AddUserViewModelResult(isCancel: true));

            await RunViewModelLifeCycle();
            await _subject.AddUserCommand.ExecuteAsync();

            _mockNavigationService.Verify(s => s.Navigate<AddUserViewModel, AddUserViewModelResult>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUserService.Verify(s => s.GetUserListAsync(), Times.Once);
            Assert.AreEqual(0, _subject.UserList.Count);
        }

    }
}
