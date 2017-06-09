using GPSTracker.Model;
using Microsoft.Phone.UserData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GPSTracker.Helpers
{
    class AppHelper
    {
        List<Friends> friends;
        AutoResetEvent sync = new AutoResetEvent(false);
        public async Task UpdateFriends()
        {
            await Task.Run(() =>
            {
                Contacts contacts = new Contacts();
                contacts.SearchAsync(String.Empty, FilterKind.None, "Friends List");
                contacts.SearchCompleted += contacts_SearchCompleted;
                sync.WaitOne();
            });
        }

        private async void contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            friends = new List<Friends>();
            foreach (Contact contact in e.Results)
            {
                foreach (ContactPhoneNumber phoneNumeber in contact.PhoneNumbers)
                {
                    friends.Add(new Friends { PhoneNumber = phoneNumeber.PhoneNumber });
                }
            }
            ServerHelper serverHelper = new ServerHelper();
            friends = await serverHelper.GetFriends(friends);
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
            {
                db.Friends.DeleteAllOnSubmit(db.Friends);
                db.SubmitChanges();
                foreach (Friends friend in friends)
                {
                    friend.ProfilePic = await serverHelper.GetProfilePic(friend.ProfilePicUrl);
                    db.Friends.InsertOnSubmit(friend);
                }
                db.SubmitChanges();
                sync.Set();
                GC.Collect();
            }
        }

    }
}
