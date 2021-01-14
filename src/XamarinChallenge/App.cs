using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using XamarinChallenge.ViewModels;

namespace XamarinChallenge
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<ISecureStorage, SecureStorageImplementation>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            RegisterAppStart<UserListViewModel>();
        }
    }
}
