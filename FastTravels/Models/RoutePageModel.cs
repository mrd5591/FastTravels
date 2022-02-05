using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FastTravel.Models
{
    public class RoutePageModel : INotifyPropertyChanged
    {
        private ObservableCollection<Route> routes;

        public ObservableCollection<Route> Routes
        {
            get { return routes; }
            set { routes = value; }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
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
