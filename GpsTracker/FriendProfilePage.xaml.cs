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

namespace GPSTracker
{
    public partial class FriendProfilePage : PhoneApplicationPage
    {
        Friends friend;
        public FriendProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
                {
                    friend = db.Friends.Where(f => f.PhoneNumber == NavigationContext.QueryString["phonenumber"].Replace(" ", "+")).FirstOrDefault();
                    this.DataContext = friend;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.OnNavigatedTo(e);
        }


    }
}