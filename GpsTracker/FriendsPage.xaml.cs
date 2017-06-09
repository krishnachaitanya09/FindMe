using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;
using GPSTracker.Model;
using GPSTracker.Helpers;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GPSTracker
{
    public partial class FriendsPage : PhoneApplicationPage
    {
        AppHelper appHelper;
        UIHelper uiHelper;
        public FriendsPage()
        {
            InitializeComponent();
            uiHelper = new UIHelper();
            appHelper = new AppHelper();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                List<Friends> friends = db.Friends.ToList();
                FriendsList.ItemsSource = friends;
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            FriendsList.ItemsSource = null;
            base.OnNavigatedFrom(e);
        }

        private void FriendsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Friends friend = e.AddedItems[0] as Friends;
            NavigationService.Navigate(new Uri("/LocationPage.xaml?phonenumber=" + friend.PhoneNumber + "&name=" + friend.Name, UriKind.Relative));
        }

        private async void Refresh_Click(object sender, EventArgs e)
        {
            ApplicationBarIconButton refreshButton = ((ApplicationBarIconButton)ApplicationBar.Buttons[0]);

            uiHelper.ShowProgressIndicator("Getting friends list......", this);
            refreshButton.IsEnabled = false;

            await appHelper.UpdateFriends();
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                List<Friends> friends = db.Friends.ToList();
                Dispatcher.BeginInvoke(() =>
                {
                    FriendsList.ItemsSource = friends;
                });
            }
            uiHelper.HideProgressIndicator(this);
            refreshButton.IsEnabled = true;
        }


    }
}