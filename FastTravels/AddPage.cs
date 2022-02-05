using Flurl;
using FastTravel.Models;
using Xamarin.Forms;
using System;

namespace FastTravel
{
    public partial class AddPage : ContentPage
    {
        public AddPageModel AddPageModel { get; set; }

        public AddPage()
        {
            InitializeComponent();

            AddPageModel = new AddPageModel();

            AddPageModel.IsEnabled = CanFindRoutes();

            BindingContext = this;
        }

        async void Search_Clicked(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;

            if((button.Equals(OriginSearch) && !string.IsNullOrWhiteSpace(AddPageModel.OriginEntry)))
            {
                GetPlace startJson = new GetPlace { search = AddPageModel.OriginEntry };

                Place startPlace;
                try
                {
                    string startPlaceResp = await Client.Post(Client.BaseUri.AppendPathSegment(Client.getPlaceEndpoint).ToUri(), startJson);
                    startPlace = Newtonsoft.Json.JsonConvert.DeserializeObject<Place>(startPlaceResp);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }

                if (startPlace == null)
                    return;

                AddPageModel.OriginName = startPlace.name;
                AddPageModel.OriginAddress = startPlace.formatted_address;
            } else if(button.Equals(DestinationSearch) && !string.IsNullOrWhiteSpace(AddPageModel.DestinationEntry))
            {
                GetPlace destinationJson = new GetPlace { search = AddPageModel.DestinationEntry };

                Place destPlace;
                try
                {
                    string destinationPlaceResp = await Client.Post(Client.BaseUri.AppendPathSegment(Client.getPlaceEndpoint).ToUri(), destinationJson);
                    destPlace = Newtonsoft.Json.JsonConvert.DeserializeObject<Place>(destinationPlaceResp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }

                if (destPlace == null)
                    return;

                AddPageModel.DestinationName = destPlace.name;
                AddPageModel.DestinationAddress = destPlace.formatted_address;
            }

            AddPageModel.IsEnabled = CanFindRoutes();
        }

        async void Routes_Clicked(System.Object sender, System.EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(AddPageModel.OriginAddress) || string.IsNullOrWhiteSpace(AddPageModel.DestinationAddress))
            {
                await DisplayAlert("Error", "Please search for both an Origin and a Destination", "OK");
                return;
            }

            GetAllRoutes routeJson = new GetAllRoutes { start = AddPageModel.OriginAddress, destination = AddPageModel.DestinationAddress };
            string routeResp = await Client.Post(Client.BaseUri.AppendPathSegment(Client.getAllRoutesEndpoint).ToUri(), routeJson);

            Directions directions;
            try
            {
                directions = Newtonsoft.Json.JsonConvert.DeserializeObject<Directions>(routeResp);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

            await Navigation.PushAsync(new RoutePage(directions, AddPageModel.OriginName + " -> " + AddPageModel.DestinationName, AddPageModel.OriginAddress, AddPageModel.DestinationAddress, AddPageModel.OriginName, AddPageModel.DestinationName));
        }

        private bool CanFindRoutes()
        {
            if(string.IsNullOrWhiteSpace(AddPageModel.OriginAddress) || string.IsNullOrWhiteSpace(AddPageModel.DestinationAddress))
            {
                return false;
            }

            return true;
        }

        void OriginEntry_Completed(System.Object sender, System.EventArgs e)
        {
            Search_Clicked(OriginSearch, null);
        }

        void DestinationEntry_Completed(System.Object sender, System.EventArgs e)
        {
            Search_Clicked(DestinationSearch, null);
        }
    }
}
