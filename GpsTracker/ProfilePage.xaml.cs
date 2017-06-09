using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GPSTracker.Model;
using GPSTracker.Helpers;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace GPSTracker
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        UserInfo userInfo;
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                userInfo = db.UserInfo.FirstOrDefault();
                this.DataContext = userInfo;
            }
        }

        private void ProfilePic_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhotoChooserTask photoChooser = new PhotoChooserTask();
            photoChooser.PixelHeight = 400;
            photoChooser.PixelWidth = 400;
            photoChooser.Show();
            photoChooser.Completed += photoChooser_Completed;
        }

        private async void photoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage pic = new BitmapImage();
                pic.SetSource(e.ChosenPhoto);
                ProfilePic.Source = pic;
                try
                {                   
                    using (MemoryStream stream = new MemoryStream())
                    {
                        e.ChosenPhoto.Position = 0;
                        e.ChosenPhoto.CopyTo(stream);
                        using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
                        {
                            db.UserInfo.FirstOrDefault().ProfilePic = stream.ToArray();
                            ServerHelper serverHelper = new ServerHelper();
                            await serverHelper.UploadProfilePic(e.ChosenPhoto);
                            db.SubmitChanges();
                        }
                    }
                }
                catch (Exception)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Unable to upload the profile picture. Please try again.");
                    });
                }
            }
        }
    }
}