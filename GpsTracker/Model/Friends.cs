using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GPSTracker.Model
{
    [Table]
    class Friends : INotifyPropertyChanged
    {
        private string _name;
        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string _phoneNumber;
        [Column(IsPrimaryKey = true, CanBeNull = false)]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                NotifyPropertyChanged("PhoneNumber");
            }
        }

        public string _email;
        [Column]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyPropertyChanged("Email");
            }
        }

        private string _profilePicUrl;
        [Column]
        public string ProfilePicUrl
        {
            get
            {
                return _profilePicUrl;
            }
            set
            {
                _profilePicUrl = value;
                NotifyPropertyChanged("ProfilePicUrl");
            }
        }

        private byte[] _profilePic;
        [Column(DbType = "image", UpdateCheck = UpdateCheck.Never)]
        public byte[] ProfilePic
        {
            get
            {
                return _profilePic;
            }
            set
            {
                _profilePic = value;
                NotifyPropertyChanged("ProfilePicStream");
            }
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
