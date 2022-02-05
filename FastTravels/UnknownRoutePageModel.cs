using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FastTravel.Models;

namespace FastTravel
{
    public class UnknownRoutePageModel : INotifyPropertyChanged
    {
        private ObservableCollection<UnknownRoute> unknownRoutes;
        public ObservableCollection<UnknownRoute> UnknownRoutes
        {
            get
            {
                return unknownRoutes;
            }

            set
            {
                unknownRoutes = value;
                NotifyPropertyChanged("UnknownRoutes");
            }
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
