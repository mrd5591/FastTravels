using System;
using System.Linq;
using FastTravels.Common;
using Plugin.FirebaseAuth;
using Plugin.FirebasePushNotification;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FastTravels
{
    public partial class App : Application
    {
        private static double Width { get; set; }
        private static double Height { get; set; }
        private static double Density { get; set; }

        public static double HeightConstant { get; private set; }
        public static double WidthConstant { get; private set; }

        public static double TextFontSize { get; private set; }
        public static double HeaderFontSize { get; private set; }

        public static DeviceIdiom DeviceIdiom { get; private set; }

        public App()
        {
            InitializeComponent();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                Width = mainDisplayInfo.Width;
                Height = mainDisplayInfo.Height;
                Density = mainDisplayInfo.Density;

                HeightConstant = Height / Density;
                WidthConstant = Width / Density;

                DeviceIdiom = DeviceInfo.Idiom;

                if (DeviceIdiom == DeviceIdiom.Tablet)
                {
                    TextFontSize = HeightConstant / 35;
                    HeaderFontSize = HeightConstant / 20;
                }
                else
                {
                    TextFontSize = HeightConstant / 45;
                    HeaderFontSize = HeightConstant / 25;
                }
            });

            bool isFirstTime = false;
            if (!Application.Current.Properties.ContainsKey("isFirstTime"))
            {
                Application.Current.Properties["isFirstTime"] = false;
                isFirstTime = true;
                Application.Current.SavePropertiesAsync();
            }

            CrossFirebaseAuth.Current.Instance.IdToken += Instance_IdToken;
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;

            MainPage = new NavigationPage(new MainPage(isFirstTime));
        }

        private async void Instance_IdToken(object sender, IdTokenEventArgs e)
        {
            if(e.Auth.CurrentUser != null)
            {
                var token = await e.Auth.CurrentUser.GetIdTokenAsync(false);
                Client.SetHttpAuthToken(token);
            }
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            if(!DependencyService.Resolve<IAuthenticationService>().IsSignedIn())
                return;

            FirebaseFunctions.UpdateFCMToken(e.Token);
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Browser.OpenAsync("https://cash.app/$mrd5591", BrowserLaunchMode.External);
        }

        async void Help_Clicked(System.Object sender, System.EventArgs e)
        {
            var actionPage = App.Current.MainPage;
            if (actionPage.Navigation != null)
                actionPage = actionPage.Navigation.NavigationStack.Last();

            string help = "";
            if(actionPage is AddPage)
            {
                help = strings.AddHelp;
            } else if(actionPage is RoutePage)
            {
                help = strings.RouteHelp;
            }

            await PopupNavigation.Instance.PushAsync(new HelpPopup(help));
        }

        async void Back_Clicked(System.Object sender, System.EventArgs e)
        {
            await this.MainPage.Navigation.PopAsync();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
