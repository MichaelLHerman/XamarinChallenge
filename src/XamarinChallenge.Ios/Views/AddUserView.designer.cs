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
	[Register ("AddUserView")]
	partial class AddUserView
	{
		[Outlet]
		UIKit.UIBarButtonItem AddButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem CancelButton { get; set; }

		[Outlet]
		UIKit.UITextField PasswordField { get; set; }

		[Outlet]
		UIKit.UITextField UserNameField { get; set; }

		[Outlet]
		UIKit.UILabel ValidationLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AddButton != null) {
				AddButton.Dispose ();
				AddButton = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (UserNameField != null) {
				UserNameField.Dispose ();
				UserNameField = null;
			}

			if (PasswordField != null) {
				PasswordField.Dispose ();
				PasswordField = null;
			}

			if (ValidationLabel != null) {
				ValidationLabel.Dispose ();
				ValidationLabel = null;
			}
		}
	}
}
