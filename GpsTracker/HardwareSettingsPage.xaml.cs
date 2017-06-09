using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GPSTracker.Helpers;
using System.Windows.Media;
using Windows.Networking.Proximity;
using Microsoft.Phone.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.Rfcomm;

namespace GPSTracker
{
    public partial class HardwareSettingsPage : PhoneApplicationPage
    {
        private BluetoothConnectionManager connectionManager;

        private StateManager stateManager;
        public HardwareSettingsPage()
        {
            InitializeComponent();
            connectionManager = new BluetoothConnectionManager();
            connectionManager.MessageReceived += connectionManager_MessageReceived;
            stateManager = new StateManager();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            connectionManager.Initialize();
            stateManager.Initialize();
        }

        private async void connectionManager_MessageReceived(string message)
        {
            string[] messagesArray = message.Split(';');
            foreach (var msg in messagesArray)
            {
                string[] messageArray = msg.Split(':');
                switch (messageArray[0])
                {
                    case "LIGHT":
                        stateManager.LightOn = messageArray[1] == "ON" ? true : false;
                        Dispatcher.BeginInvoke(delegate()
                        {
                            lightButton.Background = stateManager.LightOn ? (App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush) : new SolidColorBrush(Colors.Transparent);
                        });
                        break;
                    case "DISPLAY":
                        stateManager.DisplayOn = messageArray[1] == "ON" ? true : false;
                        Dispatcher.BeginInvoke(delegate()
                        {
                            displayButton.Background = stateManager.DisplayOn ? (App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush) : new SolidColorBrush(Colors.Transparent);
                        });
                        break;
                    case "TIME":
                        Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        await connectionManager.SendCommand("TIME:" + unixTimestamp.ToString());
                        break;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            connectionManager.Terminate();
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            connectButton.Content = "Connecting...";
            await connectionManager.Connect().ContinueWith(i =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    connectButton.Content = "Connect";

                });

            });
            string threatString = "This area is potential to threats.";
            await connectionManager.SendCommand(threatString);
        }

        private async void lightButton_Click(object sender, RoutedEventArgs e)
        {
            string command = stateManager.LightOn ? "TURN_OFF_LIGHT" : "TURN_ON_LIGHT";
            await connectionManager.SendCommand(command);
        }

        private async void displayButton_Click(object sender, RoutedEventArgs e)
        {
            string command = stateManager.DisplayOn ? "TURN_OFF_DISPLAY" : "TURN_ON_DISPLAY";
            await connectionManager.SendCommand(command);
        }

        private async void syncTimeButton_Click(object sender, RoutedEventArgs e)
        {
            Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            await connectionManager.SendCommand("TIME:" + unixTimestamp.ToString());
        }
    }
}