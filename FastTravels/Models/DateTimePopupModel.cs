using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace FastTravel.Models
{
    public class DateTimePopupModel : INotifyPropertyChanged
    {
        public Dictionary<string, bool> ActiveButtons { get; set; }

        private Color _sucolor;
        public Color SuColor
        {
            get
            {
                return _sucolor;
            }

            set
            {
                _sucolor = value;
                NotifyPropertyChanged("SuColor");
            }
        }

        private Color _mcolor;
        public Color MColor
        {
            get
            {
                return _mcolor;
            }

            set
            {
                _mcolor = value;
                NotifyPropertyChanged("MColor");
            }
        }

        private Color _tcolor;
        public Color TColor
        {
            get
            {
                return _tcolor;
            }

            set
            {
                _tcolor = value;
                NotifyPropertyChanged("TColor");
            }
        }

        private Color _wcolor;
        public Color WColor
        {
            get
            {
                return _wcolor;
            }

            set
            {
                _wcolor = value;
                NotifyPropertyChanged("WColor");
            }
        }

        private Color _rcolor;
        public Color RColor
        {
            get
            {
                return _rcolor;
            }

            set
            {
                _rcolor = value;
                NotifyPropertyChanged("RColor");
            }
        }

        private Color _fcolor;
        public Color FColor
        {
            get
            {
                return _fcolor;
            }

            set
            {
                _fcolor = value;
                NotifyPropertyChanged("FColor");
            }
        }

        private Color _sacolor;
        public Color SaColor
        {
            get
            {
                return _sacolor;
            }

            set
            {
                _sacolor = value;
                NotifyPropertyChanged("SaColor");
            }
        }

        private TimeSpan time;
        public TimeSpan Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                NotifyPropertyChanged("Time");
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
