using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FastTravel.Common;

namespace FastTravel.Models
{
    public class UnknownRoute : INotifyPropertyChanged
    {
        public string JourneyId { get; set; }
        public string JourneyName { get; set; }
        public string Summary { get; set; }
        private List<string> directions;
        public List<string> Directions
        {
            get
            {
                return directions.Select(x => HtmlUtilities.ConvertToPlainText(x)).ToList<string>();
            }

            set
            {
                directions = value;
                NotifyPropertyChanged("Directions");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
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
}
