using Foundation;
using MvvmCross.Platforms.Ios.Core;

namespace XamarinChallenge.Ios
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<MvxIosSetup<App>, App>
    {
    }
}

