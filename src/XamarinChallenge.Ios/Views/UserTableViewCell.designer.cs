// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace XamarinChallenge.Ios.Views
{
	[Register ("UserTableViewCell")]
	partial class UserTableViewCell
	{
		[Outlet]
		UIKit.UILabel PasswordLabel { get; set; }

		[Outlet]
		UIKit.UILabel UsernameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PasswordLabel != null) {
				PasswordLabel.Dispose ();
				PasswordLabel = null;
			}

			if (UsernameLabel != null) {
				UsernameLabel.Dispose ();
				UsernameLabel = null;
			}
		}
	}
}
