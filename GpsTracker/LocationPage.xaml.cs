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
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using GPSTracker.Resources;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;

namespace GPSTracker
{
    public partial class LocationPage : PhoneApplicationPage
    {
        AppSettings appSettings;
        Friends friend;
        public LocationPage()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(StackPanel));
            appSettings = new AppSettings();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                LastUpdatePanel.Visibility = System.Windows.Visibility.Collapsed;
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
                {
                    friend = db.Friends.Where(f => f.PhoneNumber == NavigationContext.QueryString["phonenumber"].Replace(" ", "+")).FirstOrDefault();
                    if(friend == null)
                    {
                        friend = new Friends();
                        friend.PhoneNumber = NavigationContext.QueryString["phonenumber"].Replace(" ", "+");
                        friend.Name = NavigationContext.QueryString["name"];
                        friend.ProfilePicUrl = "/Images/ProfilePics/default.jpg";
                    }
                }
               
                TitlePanel.DataContext = friend;

                ServerHelper serverHelper = new ServerHelper();
                UpdateMap(await serverHelper.GetFriendsLocation(friend.PhoneNumber));

                await ConnectionHelper.locationHubProxy.Invoke("JoinGroup", friend.PhoneNumber);
                ConnectionHelper.locationHubProxy.On<Location>("newLocation", location => UpdateMap(location));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ConnectionHelper.locationHubProxy.Invoke("UnJoinGroup", friend.PhoneNumber);
            base.OnNavigatedFrom(e);
        }

        private void UpdateMap(Location location)
        {
            if (location != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    LastUpdatePanel.DataContext = location;
                    LastUpdatePanel.Visibility = System.Windows.Visibility.Visible;
                    GeoCoordinate myCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
                    MyMap.Layers.Clear();
                    MapLayer mapLayer = new MapLayer();
                    // Draw marker for current position
                    if (myCoordinate != null)
                    {
                        // Create a map marker
                        Polygon polygon = new Polygon();
                        polygon.Points.Add(new Point(0, 0));
                        polygon.Points.Add(new Point(0, 75));
                        polygon.Points.Add(new Point(25, 0));
                        polygon.Fill = new SolidColorBrush(Colors.Red);

                        // Enable marker to be tapped for location information
                        polygon.Tag = new GeoCoordinate(location.Latitude, location.Longitude);

                        // Create a MapOverlay and add marker.
                        MapOverlay overlay = new MapOverlay();
                        overlay.Content = polygon;
                        overlay.GeoCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
                        overlay.PositionOrigin = new Point(0.0, 1.0);
                        mapLayer.Add(overlay);
                    }

                    MyMap.Layers.Add(mapLayer);
                    MyMap.SetView(myCoordinate, 14, MapAnimationKind.Parabolic);
                });
            }
            else
            {
                LastUpdateText.Text = "No location data available";
                LastUpdatePanel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void TitlePanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FriendProfilePage.xaml?phonenumber=" + friend.PhoneNumber + "&name=" + friend.Name, UriKind.Relative));
        }
    }
}