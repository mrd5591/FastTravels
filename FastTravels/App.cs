using System;
using FastTravel.Common;
using Plugin.FirebaseAuth;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FastTravel
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
            }


            MainPage = new NavigationPage(new MainPage(isFirstTime));

            CrossFirebaseAuth.Current.Instance.IdToken += Instance_IdToken;
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
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
