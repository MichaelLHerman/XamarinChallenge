using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XamarinChallenge.Services;

namespace XamarinChallenge.ViewModels
{
    public class UserListViewModel : MvxViewModel
    {
        #region Properties
        
        public ObservableCollection<User> UserList { get; } = new ObservableCollection<User>();
        
        #endregion

        readonly IMvxNavigationService _navigationService;
        readonly IUserService _userService;

        public UserListViewModel(IMvxNavigationService navigationService,
            IUserService userService
            )
        {
            _navigationService = navigationService;
            _userService = userService;

            AddUserCommand = new MvxAsyncCommand(AddUser);
        }

        #region LifeCycle methods

        public override void Prepare()
        {
        }

        public override async Task Initialize()
        {
            var userList = await _userService.GetUserListAsync();
            UserList.UpdateCollection(userList);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand AddUserCommand { get; }
        async Task AddUser()
        {
            var dialogResult = await _navigationService.Navigate<AddUserViewModel, AddUserViewModelResult>();

            if (!dialogResult.IsCancel)
            {
                var userList = await _userService.GetUserListAsync();
                UserList.UpdateCollection(userList);
            }
        }

        #endregion
    }

}
