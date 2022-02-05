using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FastTravel.Models
{
    public class MainPageModel : INotifyPropertyChanged
    {
        private ObservableCollection<Journey> journeys;
        public ObservableCollection<Journey> Journeys
        {
            get
            {
                return journeys;
            }

            set
            {
                journeys = value;
                NotifyPropertyChanged("Journeys");
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
