using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Navigation;
using System.Threading;
using System.Threading.Tasks;
using XamarinChallenge.Services;
using XamarinChallenge.ViewModels;

namespace XamarinChallenge.Tests
{
    [TestClass]
    public class AddUserViewModelFixture
    {
        AddUserViewModel _subject = default!;

        Mock<IUserService> _mockUserService = default!;
        Mock<IMvxNavigationService> _mockNavigationService = default!;
        Mock<ILocalizationService> _mockLocalizationService = default!;

        [TestInitialize]
        public void Setup()
        {
            _mockNavigationService = new Mock<IMvxNavigationService>();
            _mockUserService = new Mock<IUserService>();

            _mockLocalizationService = new Mock<ILocalizationService>();
            _mockLocalizationService.Setup(s => s.GetString(It.IsAny<string>())).Returns((string s) => s);

            _subject = new AddUserViewModel(_mockNavigationService.Object,
                _mockUserService.Object,
                _mockLocalizationService.Object);
        }

        [TestMethod]
        public void AddUser_NoInput_CanNotAdd()
        {
            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordLessThan4_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "pas1";

            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordGreaterThan12_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "longenougher1"; 

            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordOnlyLetters_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "longenough";

            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordOnlyNumbers_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "123456";

            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordHasInvalidChar_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "a.123456";

            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordHasConsecutiveRepeat_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "a123123";

            Assert.IsFalse(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public void AddUser_PasswordHasNonConsecutiveRepeat_CanAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "a1234123";

            Assert.IsTrue(_subject.AddUserCommand.CanExecute());
        }

        [TestMethod]
        public async Task AddUserCommand_Valid_CallsUserServiceAndCloses()
        {
            _mockUserService.Setup(s => s.AddUserAsync(It.IsAny<string>(), It.IsAny<string>()));
            _mockNavigationService.Setup(s => s.Close(It.IsAny<AddUserViewModel>(), It.IsAny<AddUserViewModelResult>(), It.IsAny<CancellationToken>()));

            _subject.UserName = "a";
            _subject.Password = "a1234123";

            Assert.IsTrue(_subject.AddUserCommand.CanExecute());
            await _subject.AddUserCommand.ExecuteAsync();

            _mockUserService.Verify(s => s.AddUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockNavigationService.Verify(s => s.Close(It.IsAny<AddUserViewModel>(), It.Is<AddUserViewModelResult>(r => !r.IsCancel), It.IsAny<CancellationToken>()), Times.Once);

        }

        [TestMethod]
        public async Task Cancel_CallsUserServiceAndCloses()
        {
            _mockUserService.Setup(s => s.AddUserAsync(It.IsAny<string>(), It.IsAny<string>()));
            _mockNavigationService.Setup(s => s.Close(It.IsAny<AddUserViewModel>(), It.IsAny<AddUserViewModelResult>(), It.IsAny<CancellationToken>()));

            Assert.IsTrue(_subject.CancelCommand.CanExecute());
            await _subject.CancelCommand.ExecuteAsync();

            _mockUserService.Verify(s => s.AddUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockNavigationService.Verify(s => s.Close(It.IsAny<AddUserViewModel>(), It.Is<AddUserViewModelResult>(r => r.IsCancel), It.IsAny<CancellationToken>()), Times.Once);

        }




        [TestMethod]
        public void ValidateAndAddCommandExecute_NoInput_ShowsValidationMessage()
        {
            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_UserNameIsRequired", _subject.ValidationMessage);
        }

        [TestMethod]
        public void ValidateAndAddCommandExecute_PasswordLessThan4_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "pas1";

            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_PasswordLengthOutOfRange", _subject.ValidationMessage);
        }

        [TestMethod]
        public void ValidateAndAddCommandExecute_PasswordGreaterThan12_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "longenougher1";

            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_PasswordLengthOutOfRange", _subject.ValidationMessage);
        }

        [TestMethod]
        public void ValidateAndAddCommandExecute_PasswordOnlyLetters_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "longenough";

            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_PasswordMustContainLettersAndNumbers", _subject.ValidationMessage);
        }

        [TestMethod]
        public void ValidateAndAddCommandExecute_PasswordOnlyNumbers_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "123456";

            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_PasswordMustContainLettersAndNumbers", _subject.ValidationMessage);
        }

        [TestMethod]
        public void ValidateAndAddCommandExecute_PasswordHasInvalidChar_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "a.123456";

            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_InvalidPasswordCharacter", _subject.ValidationMessage);
        }

        [TestMethod]
        public void ValidateAndAddCommandExecute_PasswordHasConsecutiveRepeat_CanNotAdd()
        {
            _subject.UserName = "a";
            _subject.Password = "a123123";

            _subject.ValidateAndAddCommand.Execute();
            Assert.AreEqual("AddUser_PasswordMustNotContainRepeatedSequence", _subject.ValidationMessage);
        }


        [TestMethod]
        public void ValidateAndAddCommandExecute_Valid_CallsUserServiceAndCloses()
        {
            _mockUserService.Setup(s => s.AddUserAsync(It.IsAny<string>(), It.IsAny<string>()));
            _mockNavigationService.Setup(s => s.Close(It.IsAny<AddUserViewModel>(), It.IsAny<AddUserViewModelResult>(), It.IsAny<CancellationToken>()));

            _subject.UserName = "a";
            _subject.Password = "a1234123";

            _subject.ValidateAndAddCommand.Execute();
            Assert.IsNull(_subject.ValidationMessage);

            _mockUserService.Verify(s => s.AddUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockNavigationService.Verify(s => s.Close(It.IsAny<AddUserViewModel>(), It.Is<AddUserViewModelResult>(r => !r.IsCancel), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
