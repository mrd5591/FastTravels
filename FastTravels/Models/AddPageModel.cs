using System;
using System.ComponentModel;

namespace FastTravel.Models
{
    public class AddPageModel : INotifyPropertyChanged
    {
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

        private string _originEntry;
        public string OriginEntry
        {
            get
            {
                return _originEntry;
            }

            set
            {
                _originEntry = value;
                NotifyPropertyChanged("OriginEntry");
            }
        }

        private string _destinationEntry;
        public string DestinationEntry
        {
            get
            {
                return _destinationEntry;
            }

            set
            {
                _destinationEntry = value;
                NotifyPropertyChanged("DestinationEntry");
            }
        }

        private string _originName;
        public string OriginName
        {
            get
            {
                return _originName;
            }

            set
            {
                _originName = value;
                NotifyPropertyChanged("OriginName");
            }
        }

        private string _originAddress;
        public string OriginAddress
        {
            get
            {
                return _originAddress;
            }

            set
            {
                _originAddress = value;
                NotifyPropertyChanged("OriginAddress");
            }
        }

        private string _destinationName;
        public string DestinationName
        {
            get
            {
                return _destinationName;
            }

            set
            {
                _destinationName = value;
                NotifyPropertyChanged("DestinationName");
            }
        }

        private string _destinationAddress;
        public string DestinationAddress
        {
            get
            {
                return _destinationAddress;
            }

            set
            {
                _destinationAddress = value;
                NotifyPropertyChanged("DestinationAddress");
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
