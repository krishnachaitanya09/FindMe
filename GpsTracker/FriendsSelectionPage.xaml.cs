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

namespace GPSTracker
{
    public partial class FriendsSelectionPage : PhoneApplicationPage
    {
        List<Friends> friends;
        public FriendsSelectionPage()
        {
            InitializeComponent();
            this.ApplicationBar = (ApplicationBar)this.Resources["applicationBar1"];
            friends = new List<Friends>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                HashSet<String> blockList = new HashSet<string>(db.BlockList.Select(i => i.PhoneNumber));
                friends = db.Friends.Where(f => !blockList.ToList().Contains(f.PhoneNumber)).ToList();
                friendsList.ItemsSource = friends;
            }
            base.OnNavigatedTo(e);
        }

        private void FriendsList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (friendsList.IsSelectionEnabled == true)
            {
                this.ApplicationBar = (ApplicationBar)this.Resources["applicationBar2"];
            }
            else
            {
                this.ApplicationBar = (ApplicationBar)this.Resources["applicationBar1"];
            }
        }

        private void Select_Click(object sender, EventArgs e)
        {
            friendsList.IsSelectionEnabled = true;
        }

        private async void Done_Click(object sender, EventArgs e)
        {
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                foreach (Friends blockedFriend in friendsList.SelectedItems)
                {
                    db.BlockList.InsertOnSubmit(new BlockList() { PhoneNumber = blockedFriend.PhoneNumber });
                }
                db.SubmitChanges();
                ServerHelper serverHelper = new ServerHelper();
                await serverHelper.SendBlockList(db.BlockList.ToList());
            }            
            NavigationService.GoBack();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (friendsList.IsSelectionEnabled == true)
            {
                e.Cancel = true;
                friendsList.IsSelectionEnabled = false;
            }
            base.OnBackKeyPress(e);
        }

        private void FriendsList_LayoutUpdated(object sender, EventArgs e)
        {
            if (friendsList.ItemsSource.Count == 0)
            {
                ((ApplicationBarIconButton)((ApplicationBar)this.Resources["applicationBar1"]).Buttons[0]).IsEnabled = false;
            }
            else
            {
                ((ApplicationBarIconButton)((ApplicationBar)this.Resources["applicationBar1"]).Buttons[0]).IsEnabled = true;
            }
        }
    }
}