using GPSTracker.Model;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GPSTracker.Helpers
{
    static class ConnectionHelper
    {
        static AppSettings appSettings;
        public static HubConnection hubConnection;
        public static IHubProxy locationHubProxy;
        public static bool isConnected { get; set; }

        static ConnectionHelper()
        {
            appSettings = new AppSettings();
            hubConnection = new HubConnection("http://techcryptic.com/findme");
            hubConnection.Headers.Add(new KeyValuePair<string, string>("Authorization", "Bearer " + appSettings.GetValue<String>("token")));
            locationHubProxy = hubConnection.CreateHubProxy("LocationHub");
        }

        public static async Task Connect()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                await hubConnection.Start();
                hubConnection.StateChanged += hubConnection_StateChanged;
            }
        }
        static async void hubConnection_StateChanged(StateChange obj)
        {
            if (obj.NewState == ConnectionState.Connected)
            {
                isConnected = true;
            }
            else
            {
                isConnected = false;
                await hubConnection.Start();
            }
        }
    }
}
