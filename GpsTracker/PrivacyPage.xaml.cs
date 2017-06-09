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
using System.Collections.ObjectModel;
using GPSTracker.Helpers;

namespace GPSTracker
{
    public partial class PrivacyPage : PhoneApplicationPage
    {
        List<Friends> friends;
        public PrivacyPage()
        {
            InitializeComponent();
            this.ApplicationBar = (ApplicationBar)this.Resources["applicationBar1"];
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                HashSet<String> blockList = new HashSet<string>(db.BlockList.Select(i => i.PhoneNumber));
                friends = new List<Friends>(db.Friends.Where(f => blockList.ToList().Contains(f.PhoneNumber)));
                BlockList.ItemsSource = friends;
            }
            base.OnNavigatedTo(e);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FriendsSelectionPage.xaml", UriKind.Relative));
        }

        private void BlockList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (BlockList.IsSelectionEnabled == true)
            {
                this.ApplicationBar = (ApplicationBar)this.Resources["applicationBar2"];
            }
            else
            {
                this.ApplicationBar = (ApplicationBar)this.Resources["applicationBar1"];
            }
        }

        private void BlockList_LayoutUpdated(object sender, EventArgs e)
        {
            if (BlockList.ItemsSource.Count == 0)
            {
                ((ApplicationBarIconButton)((ApplicationBar)this.Resources["applicationBar1"]).Buttons[1]).IsEnabled = false;
            }
            else
            {
                ((ApplicationBarIconButton)((ApplicationBar)this.Resources["applicationBar1"]).Buttons[1]).IsEnabled = true;
            }
        }

        private void Select_Click(object sender, EventArgs e)
        {
            BlockList.IsSelectionEnabled = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (BlockList.IsSelectionEnabled == true)
            {
                e.Cancel = true;
                BlockList.IsSelectionEnabled = false;
            }
            base.OnBackKeyPress(e);
        }

        private async void Remove_Click(object sender, EventArgs e)
        {
            List<BlockList> unBlockedFriends = new List<BlockList>();
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                foreach (Friends friend in BlockList.SelectedItems)
                {                    
                    BlockList.ItemsSource.Remove(friend);                    
                    BlockList unBlock = new BlockList(){
                         PhoneNumber = friend.PhoneNumber
                    };
                    unBlockedFriends.Add(unBlock);
                    db.BlockList.Attach(unBlock);
                    db.BlockList.DeleteOnSubmit(unBlock);                    
                }
                BlockList.ItemsSource = null;
                BlockList.ItemsSource = friends;                               
                db.SubmitChanges();
                ServerHelper serverHelper = new ServerHelper();
                await serverHelper.RemoveFromBlockList(unBlockedFriends);
            }
        }
    }
}