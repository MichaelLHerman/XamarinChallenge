using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.WeakSubscription;
using UIKit;
using XamarinChallenge.ViewModels;

namespace XamarinChallenge.Ios.Views
{
	[MvxFromStoryboard("UserList")]
	[MvxModalPresentation(ModalPresentationStyle = UIModalPresentationStyle.FormSheet, WrapInNavigationController = true)]
	public partial class AddUserView : MvxViewController
	{
        IDisposable _userNameEditingDidEndOnExitSubscription;

        public AddUserView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<AddUserView, AddUserViewModel>();

            set.Bind(AddButton).For(v => v.BindClicked()).To(vm => vm.AddUserCommand);
            set.Bind(CancelButton).For(v => v.BindClicked()).To(vm => vm.CancelCommand);
            set.Bind(UserNameField).To(vm => vm.UserName);
            _userNameEditingDidEndOnExitSubscription = UserNameField.WeakSubscribe(nameof(UserNameField.EditingDidEndOnExit), UserNameField_EditingDidEndOnExit);
            set.Bind(PasswordField).To(vm => vm.Password);
            set.Bind(PasswordField).For(v => v.BindEditingDidEndOnExit()).To(vm => vm.ValidateAndAddCommand);
            set.Bind(ValidationLabel).To(vm => vm.ValidationMessage);

            set.Apply();

            UserNameField.BecomeFirstResponder();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userNameEditingDidEndOnExitSubscription != null)
                {
                    _userNameEditingDidEndOnExitSubscription.Dispose();
                }
                ReleaseDesignerOutlets();
            }
            base.Dispose(disposing);
        }

        void UserNameField_EditingDidEndOnExit(object sender, EventArgs e)
        {
            PasswordField.BecomeFirstResponder();
        }
    }
}
