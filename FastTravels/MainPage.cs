using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FastTravels.Common;
using FastTravels.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.FirebasePushNotification;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FastTravels
{
    public partial class MainPage : ContentPage
    {
        public MainPageModel MainPageModel { get; set; }

        private bool isFirstTime;

        public MainPage(bool isFirstTime)
        {
            InitializeComponent();

            this.isFirstTime = isFirstTime;
            MainPageModel = new MainPageModel();

            var authenticationService = DependencyService.Resolve<IAuthenticationService>();

            if (!authenticationService.IsSignedIn())
            {
                Client.SignInAnonymously();
            }
            else
            {
                Client.Login();
            }

            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;

            LoadData();

            BindingContext = this;
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            DisplayAlert(e.Data["aps.alert.title"].ToString(), e.Data["aps.alert.body"].ToString(), "Ok");
        }

        async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPage());
        }

        async void LoadData()
        {
            MainPageModel.Journeys = new ObservableCollection<Journey>(await FirebaseFunctions.GetJourneys());

            List<UnknownRoute> unknownRoutes = await FirebaseFunctions.GetUnknownRoutes();

            if(unknownRoutes.Count != 0)
            {
                bool answer;
                if(unknownRoutes.Count == 1)
                {
                    answer = await DisplayAlert("Unknown Route Found", "An unknown route was discovered during the execution of one of your notifications. Would you like to give it a nickname now?", "Yes", "No");
                } else
                {
                    answer = await DisplayAlert("Unknown Routes Found", "Unknown routes were discovered during the execution of one or many of your notifications. Would you like to give them nicknames now?", "Yes", "No");
                }

                if (answer)
                {
                    await Navigation.PushAsync(new UnknownRoutePage(unknownRoutes));
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (isFirstTime)
            {
                await DisplayAlert("Welcome", "Pick a start and destination, name the routes, and set your schedule. You'll get notifications of the fastest route on your schedule. Add your journey around the time of your schedule to get the best results.", "Ok");
                isFirstTime = false;
            }

            LoadData();
        }

        void Delete_Clicked(object sender, EventArgs e)
        {
            Journey journey = (Journey)((ImageButton)sender).CommandParameter;
            journey.IsActive = false;
            MainPageModel.Journeys.Remove(journey);
            FirebaseFunctions.DeleteJourney(journey);
        }

        async void Edit_Clicked(object sender, EventArgs e)
        {
            Journey journey = (Journey)((ImageButton)sender).CommandParameter;
            await PopupNavigation.Instance.PushAsync(new DateTimePopup(journey));
        }
    }
}
