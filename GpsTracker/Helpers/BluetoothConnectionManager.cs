using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace GPSTracker.Helpers
{
    /// <summary>
    /// Class to control the bluetooth connection to the Arduino.
    /// </summary>
    public class BluetoothConnectionManager
    {
        /// <summary>
        /// Socket used to communicate with Arduino.
        /// </summary>
        private StreamSocket socket;

        /// <summary>
        /// DataWriter used to send commands easily.
        /// </summary>
        private DataWriter dataWriter;

        /// <summary>
        /// DataReader used to receive messages easily.
        /// </summary>
        private DataReader dataReader;

        /// <summary>
        /// Thread used to keep reading data from socket.
        /// </summary>
        private BackgroundWorker dataReadWorker;

        /// <summary>
        /// Delegate used by event handler.
        /// </summary>
        /// <param name="message">The message received.</param>
        public delegate void MessageReceivedHandler(string message);

        /// <summary>
        /// Event fired when a new message is received from Arduino.
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        /// <summary>
        /// Initialize the manager, should be called in OnNavigatedTo of main page.
        /// </summary>
        public void Initialize()
        {
            socket = new StreamSocket();
            dataReadWorker = new BackgroundWorker();
            dataReadWorker.WorkerSupportsCancellation = true;
            dataReadWorker.DoWork += new DoWorkEventHandler(ReceiveMessages);
        }

        /// <summary>
        /// Finalize the connection manager, should be called in OnNavigatedFrom of main page.
        /// </summary>
        public void Terminate()
        {
            if (socket != null)
            {
                socket.Dispose();
                socket = null;
            }
            if (dataWriter != null)
            {
                dataWriter.DetachStream();
                dataWriter = null;
            }
            if (dataReadWorker != null)
            {
                dataReadWorker.CancelAsync();
            }
        }

        /// <summary>
        /// Connect to the given host device.
        /// </summary>
        /// <param name="deviceHostName">The host device name.</param>
        public async Task Connect()
        {
            try
            {
                PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "{00001101-0000-1000-8000-00805f9b34fb}";
                var pairedDevices = await PeerFinder.FindAllPeersAsync();

                if (pairedDevices.Count == 0)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("No paired devices were found.");
                    });
                }
                else
                {
                    foreach (PeerInformation pairedDevice in pairedDevices)
                    {
                        if (pairedDevice.DisplayName == "HC-05")
                        {
                            if (socket != null)
                            {
                                await socket.ConnectAsync(pairedDevice.HostName, "1", SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
                                dataReader = new DataReader(socket.InputStream);
                                dataReadWorker.RunWorkerAsync();
                                dataWriter = new DataWriter(socket.OutputStream);
                                await SendCommand("SYNC_DATA");
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80070490)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (MessageBox.Show("Bluetooth device is not connected. Click OK to open Bluetooth settings.", "Alert!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
                            connectionSettingsTask.Show();
                        }
                    });
                }
                else if ((uint)ex.HResult == 0x80070103)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (MessageBox.Show("Bluetooth device is not available or turned off. Click OK to open Bluetooth settings.", "Alert!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
                            connectionSettingsTask.Show();
                        }
                    });
                }
                else if ((uint)ex.HResult == 0x8007048F)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (MessageBox.Show("Bluetooth is turned off. Click OK to open Bluetooth settings.", "Alert!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
                            connectionSettingsTask.Show();
                        }
                    });
                }
            }
            
        }

        /// <summary>
        /// Receive messages from the Arduino through bluetooth.
        /// </summary>
        private async void ReceiveMessages(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    // Read first byte (length of the subsequent message, 255 or less). 
                    uint sizeFieldCount = await dataReader.LoadAsync(1);
                    if (sizeFieldCount != 1)
                    {
                        // The underlying socket was closed before we were able to read the whole data. 
                        return;
                    }

                    // Read the message. 
                    uint messageLength = dataReader.ReadByte();
                    uint actualMessageLength = await dataReader.LoadAsync(messageLength);
                    if (messageLength != actualMessageLength)
                    {
                        // The underlying socket was closed before we were able to read the whole data. 
                        return;
                    }
                    // Read the message and process it.
                    string message = dataReader.ReadString(actualMessageLength);
                    MessageReceived(message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Send command to the Arduino through bluetooth.
        /// </summary>
        /// <param name="command">The sent command.</param>
        /// <returns>The number of bytes sent</returns>
        public async Task<uint> SendCommand(string command)
        {
            uint sentCommandSize = 0;
            if (dataWriter != null)
            {
                uint commandSize = dataWriter.MeasureString(command);
                dataWriter.WriteByte((byte)commandSize);
                sentCommandSize = dataWriter.WriteString(command);
                await dataWriter.StoreAsync();
            }
            return sentCommandSize;
        }
    }
}
