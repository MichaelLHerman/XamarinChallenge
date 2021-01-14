using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;
using XamarinChallenge.ViewModels;

namespace XamarinChallenge.Ios.Views
{
    [MvxFromStoryboard("UserList")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class UserListView : MvxTableViewController
	{
        //retain since MvxSimpleTableViewSource's managed reference UITableView is weak and was getting GC'ed
        UITableView _tableView;

		public UserListView (IntPtr handle) : base (handle)
		{
			
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _tableView = TableView;

            var set = this.CreateBindingSet<UserListView, UserListViewModel>();

            var source = new MvxSimpleTableViewSource(TableView, null, UserTableViewCell.Key, registerNibForCellReuse: false)
            {
                UseAnimations = true
            };
            
            set.Bind(source).To(vm => vm.UserList);
            set.Bind(AddButton).For(v => v.BindClicked()).To(vm => vm.AddUserCommand);            
            set.Apply();

            TableView.Source = source;
            TableView.ReloadData();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tableView = null;
                ReleaseDesignerOutlets();
            }
            base.Dispose(disposing);
        }
    }
}

