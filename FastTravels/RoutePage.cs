using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FastTravel.Common;
using FastTravel.Models;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FastTravel
{
    public partial class RoutePage : ContentPage
    {
        public RoutePageModel RoutePageModel { get; set; }
        private string journeyName;
        private string origin;
        private string destination;
        private string originName;
        private string destinationName;

        public RoutePage(Directions _directions, string _name, string _origin, string _destination, string _originName, string _destinationName)
        {
            InitializeComponent();

            RoutePageModel = new RoutePageModel();
            journeyName = _name;
            origin = _origin;
            destination = _destination;
            originName = _originName;
            destinationName = _destinationName;

            RoutePageModel.Routes = new ObservableCollection<Route>(_directions.routes);

            BindingContext = this;
        }

        void CarouselView_Scrolled(System.Object sender, Xamarin.Forms.ItemsViewScrolledEventArgs e)
        {
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!AreEntriesReady(out int failCause))
            {
                switch (failCause)
                {
                    case 1:
                        await DisplayAlert("Error", "No routes available.", "OK");
                        break;
                    case 2:
                    case 3:
                        await DisplayAlert("Error", "Please give a unique name to all routes.", "OK");
                        break;
                }
                return;
            }

            Journey journey = new Journey { Id = Guid.NewGuid().ToString(), Routes = RoutePageModel.Routes.ToList(), Name = journeyName, Origin = origin, Destination = destination, OriginName = originName, DestinationName = destinationName };

            await PopupNavigation.Instance.PushAsync(new DateTimePopup(journey));

            FirebaseFunctions.CreateJourneys(journey);
        }

        private bool AreEntriesReady(out int failCause)
        {
            if(RoutePageModel.Routes.Count == 0)
            {
                failCause = 1;
                return false;
            }

            HashSet<string> names = new HashSet<string>();
            foreach (Route route in RoutePageModel.Routes)
            {
                if (string.IsNullOrWhiteSpace(route.Name))
                {
                    failCause = 2;
                    return false;
                }

                names.Add(route.Name);
            }

            if(names.Count != RoutePageModel.Routes.Count)
            {
                failCause = 3;
                return false;
            }

            failCause = 0;
            return true;
        }

        void NameEntry_Completed(System.Object sender, System.EventArgs e)
        {
            Carousel.Position = NextAvailableEntry();
        }

        private int NextAvailableEntry()
        {
            int i = Carousel.Position;

            for(int j = i; j < RoutePageModel.Routes.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(RoutePageModel.Routes[j].Name))
                {
                    return j;
                }
            }

            for (int j = 0; j < i; j++)
            {
                if (string.IsNullOrWhiteSpace(RoutePageModel.Routes[j].Name))
                {
                    return j;
                }
            }

            return i;
        }

        void NameEntry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            ((Entry)sender).Text = ((Entry)sender)?.Text?.Trim();
        }

        void NameEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            RoutePageModel.IsEnabled = AreEntriesReady(out int failCause);
        }
    }
}
