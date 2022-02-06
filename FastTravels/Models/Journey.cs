using System;
using System.Collections.Generic;
using System.ComponentModel;
using FastTravels.Common;
using Plugin.CloudFirestore.Attributes;
using Xamarin.Forms;

namespace FastTravels.Models
{
    public class Journey : INotifyPropertyChanged
    {
        [Id]
        public string Id { get; set; }
        public List<Route> Routes { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public string Time { get; set; }
        public List<string> ActiveDays { get; set; }

        private bool isActive;
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                NotifyPropertyChanged("IsActive");
                NotifyPropertyChanged("CheckboxColor");
                FirebaseFunctions.UpdateJourneyIsActive(this);
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                NotifyPropertyChanged("Name");

            }
        }

        [Ignored]
        public Color CheckboxColor
        {
            get
            {
                return IsActive ? Color.Green : Color.Red;
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum Day
    {
        SUN,
        MON,
        TUE,
        WED,
        THU,
        FRI,
        SAT,
    }
}
