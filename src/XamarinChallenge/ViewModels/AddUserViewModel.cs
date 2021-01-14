using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XamarinChallenge.Services;

namespace XamarinChallenge.ViewModels
{
    public sealed class AddUserViewModelResult
    {
        public bool IsCancel { get; }

        public AddUserViewModelResult(bool isCancel)
        {
            IsCancel = isCancel;
        }
    }

    public sealed class AddUserViewModel : MvxViewModelResult<AddUserViewModelResult>
    {
        #region Properties

        string _userName = string.Empty;
        public string UserName 
        {
            get => _userName;
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    ValidationMessage = String.Empty;
                    AddUserCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string _password = string.Empty;
        public string Password 
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    ValidationMessage = String.Empty;
                    AddUserCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string? _validationMessage;
        public string? ValidationMessage
        {
            get => _validationMessage;
            private set => SetProperty(ref _validationMessage, value);
        }

        #endregion

        readonly IMvxNavigationService _navigationService;
        readonly IUserService _userService;
        readonly ILocalizationService _localizationService;

        readonly Regex _hasRepeatedSubstringRegex = new Regex(@"(\w+)\1");

        public AddUserViewModel(IMvxNavigationService navigationService,
            IUserService userService,
            ILocalizationService localizationService
            )
        {
            _navigationService = navigationService;
            _userService = userService;
            _localizationService = localizationService;

            AddUserCommand = new MvxAsyncCommand(AddUser, CanAddUser);
            CancelCommand = new MvxAsyncCommand(Cancel);
            ValidateAndAddCommand = new MvxCommand(ValidateAndAdd);
        }

        #region Commands

        public IMvxAsyncCommand AddUserCommand { get; }
        bool CanAddUser()
        {
            return GetValidationMessage() is null;
        }

        async Task AddUser()
        {
            await _userService.AddUserAsync(UserName, Password);
            await _navigationService.Close(this, new AddUserViewModelResult(false));
        }

        public IMvxAsyncCommand CancelCommand { get; }
        async Task Cancel()
        {
            await _navigationService.Close(this, new AddUserViewModelResult(isCancel: true));
        }

        public IMvxCommand ValidateAndAddCommand { get; }
        void ValidateAndAdd()
        {
            ValidationMessage = GetValidationMessage();
            if (AddUserCommand.CanExecute())
            {
                AddUserCommand.Execute();
            }
        }

        string? GetValidationMessage()
        {
            if (String.IsNullOrWhiteSpace(UserName))
            {
                return _localizationService.GetString("AddUser_UserNameIsRequired");
            }
            if (Password.Length < 5 || Password.Length > 12)
            {
                return _localizationService.GetString("AddUser_PasswordLengthOutOfRange");
            }

            if (Password.Any(c => !Char.IsLetterOrDigit(c)))
            {
                return _localizationService.GetString("AddUser_InvalidPasswordCharacter");
            }

            if (!Password.Any(c => Char.IsLetter(c)) || !Password.Any(c => Char.IsDigit(c)))
            {
                return _localizationService.GetString("AddUser_PasswordMustContainLettersAndNumbers");
            }

            if (_hasRepeatedSubstringRegex.IsMatch(Password))
            {
                return _localizationService.GetString("AddUser_PasswordMustNotContainRepeatedSequence");
            }

            return null;
        }

        #endregion
    }
}
