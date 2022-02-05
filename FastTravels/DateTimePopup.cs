
using System.Threading.Tasks;
using FastTravel.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;
using System;
using FastTravel.Common;
using Rg.Plugins.Popup.Services;

namespace FastTravel
{
    public partial class DateTimePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public DateTimePopupModel DateTimePopupModel { get; set; }
        private Journey Journey;

        public DateTimePopup(Journey journey)
        {
            InitializeComponent();

            DateTimePopupModel = new DateTimePopupModel();

            if(journey.ActiveDays == null)
            {
                journey.ActiveDays = new List<string>();
            }

            DateTimePopupModel.SuColor = journey.ActiveDays.Contains(Day.SUN.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
            DateTimePopupModel.MColor = journey.ActiveDays.Contains(Day.MON.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
            DateTimePopupModel.TColor = journey.ActiveDays.Contains(Day.TUE.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
            DateTimePopupModel.WColor = journey.ActiveDays.Contains(Day.WED.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
            DateTimePopupModel.RColor = journey.ActiveDays.Contains(Day.THU.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
            DateTimePopupModel.FColor = journey.ActiveDays.Contains(Day.FRI.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
            DateTimePopupModel.SaColor = journey.ActiveDays.Contains(Day.SAT.ToString()) ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");

            DateTimePopupModel.ActiveButtons = new Dictionary<string, bool>();
            DateTimePopupModel.ActiveButtons.Add("SUN", journey.ActiveDays.Contains("SUN"));
            DateTimePopupModel.ActiveButtons.Add("MON", journey.ActiveDays.Contains("MON"));
            DateTimePopupModel.ActiveButtons.Add("TUE", journey.ActiveDays.Contains("TUE"));
            DateTimePopupModel.ActiveButtons.Add("WED", journey.ActiveDays.Contains("WED"));
            DateTimePopupModel.ActiveButtons.Add("THU", journey.ActiveDays.Contains("THU"));
            DateTimePopupModel.ActiveButtons.Add("FRI", journey.ActiveDays.Contains("FRI"));
            DateTimePopupModel.ActiveButtons.Add("SAT", journey.ActiveDays.Contains("SAT"));

            if (journey.Time != null)
            {
                string[] pieces = journey.Time.Split(':');

                if(pieces.Length != 3)
                {
                    DateTimePopupModel.Time = new TimeSpan(7, 0, 0);
                }
                else
                {
                    DateTimePopupModel.Time = new TimeSpan(int.Parse(pieces[0]), int.Parse(pieces[1]), int.Parse(pieces[2]));
                }
            }
            else
            {
                DateTimePopupModel.Time = new TimeSpan(7, 0, 0);
            }

            Journey = journey;

            BindingContext = this;
        }

        void Day_Clicked(System.Object sender, System.EventArgs e)
        {
            Button day = (Button)sender;

            bool newVal;
            if (DateTimePopupModel.ActiveButtons.Keys.Contains(day.ClassId.ToString()))
            {
                newVal = !DateTimePopupModel.ActiveButtons[day.ClassId.ToString()];
                DateTimePopupModel.ActiveButtons[day.ClassId] = newVal;
            } else
            {
                newVal = true;
                DateTimePopupModel.ActiveButtons.Add(day.ClassId.ToString(), newVal);
            }

            day.BackgroundColor = newVal ? Color.FromHex("#000d60") : Color.FromHex("#d9d9d9");
        }

        async void Save_Clicked(System.Object sender, System.EventArgs e)
        {
            Journey.ActiveDays = new List<string>();
            foreach(string d in DateTimePopupModel.ActiveButtons.Keys)
            {
                Day day;
                if(Enum.TryParse(d, out day))
                {
                    if (DateTimePopupModel.ActiveButtons[day.ToString()])
                    {
                        Journey.ActiveDays.Add(day.ToString());
                    } 
                }
            }

            if(Journey.ActiveDays.Count == 0)
            {
                return;
            }

            Journey.Time = _timePicker.Time.ToString();
            Journey.IsActive = true;

            FirebaseFunctions.UpdateJourneyDateTime(Journey);

            ClosePopup();
            await Navigation.PopToRootAsync();
        }

        void Cancel_Clicked(object sender, System.EventArgs e)
        {
            ClosePopup();
        }

        void ClosePopup()
        {
            PopupNavigation.Instance.RemovePageAsync(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}
