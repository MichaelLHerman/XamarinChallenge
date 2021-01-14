using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace XamarinChallenge.Ios.Views
{
	public partial class UserTableViewCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("UserTableViewCell");

		public UserTableViewCell(IntPtr handle) : base(handle)
		{
			this.DelayBind(() =>
			{
				var set = this.CreateBindingSet<UserTableViewCell, User>();
				set.Bind(UsernameLabel).To(m => m.UserName);
				set.Bind(PasswordLabel).To(m => m.Password);
				set.Apply();
			});
		}
	}
}
