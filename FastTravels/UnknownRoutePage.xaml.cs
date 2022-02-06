using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FastTravels.Common;
using FastTravels.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FastTravels
{
    public partial class UnknownRoutePage : ContentPage
    {
        public UnknownRoutePageModel UnknownRoutePageModel { get; set; }
        public UnknownRoutePage(List<UnknownRoute> unknownRoutes)
        {
            InitializeComponent();

            UnknownRoutePageModel = new UnknownRoutePageModel();

            UnknownRoutePageModel.UnknownRoutes = new ObservableCollection<UnknownRoute>(unknownRoutes);

            BindingContext = this;
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

            FirebaseFunctions.SetUnknownRoutes(UnknownRoutePageModel.UnknownRoutes.ToList<UnknownRoute>());
            await Navigation.PopToRootAsync();
        }

        private bool AreEntriesReady(out int failCause)
        {
            if (UnknownRoutePageModel.UnknownRoutes.Count == 0)
            {
                failCause = 1;
                return false;
            }

            HashSet<string> names = new HashSet<string>();
            foreach (UnknownRoute route in UnknownRoutePageModel.UnknownRoutes)
            {
                if (string.IsNullOrWhiteSpace(route.Name))
                {
                    failCause = 2;
                    return false;
                }

                names.Add(route.Name);
            }

            if (names.Count != UnknownRoutePageModel.UnknownRoutes.Count)
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

            for (int j = i; j < UnknownRoutePageModel.UnknownRoutes.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(UnknownRoutePageModel.UnknownRoutes[j].Name))
                {
                    return j;
                }
            }

            for (int j = 0; j < i; j++)
            {
                if (string.IsNullOrWhiteSpace(UnknownRoutePageModel.UnknownRoutes[j].Name))
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
            UnknownRoutePageModel.IsEnabled = AreEntriesReady(out int failCause);
        }
    }
}
