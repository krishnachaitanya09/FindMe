using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.ComponentModel;

namespace GPSTracker.Controls
{
    public partial class OnlineStatusControl : UserControl, INotifyPropertyChanged
    {
        private bool _isOnline;
        public bool IsOnline
        {
            get
            {
                return _isOnline;
            }
            set
            {
                _isOnline = value;
                Dispatcher.BeginInvoke(() =>
                {
                    if (_isOnline)
                    {
                        this.Color = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        this.Color = new SolidColorBrush(Colors.Gray);
                    }
                });
                NotifyPropertyChanged("IsOnline");
            }
        }

        private Brush _color;
        public Brush Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        public OnlineStatusControl()
        {
            InitializeComponent();
            this.IsOnline = false;
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }   
}
