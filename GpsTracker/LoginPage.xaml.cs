using GPSTracker.Helpers;
using GPSTracker.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace GPSTracker
{
    public partial class LoginPage : PhoneApplicationPage
    {
        ServerHelper serverHelper;
        UIHelper uiHelper;
        AppHelper appHelper;

        public LoginPage()
        {
            InitializeComponent();
            uiHelper = new UIHelper();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            base.OnNavigatedFrom(e);
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            this.Pivot.IsHitTestVisible = false;
            this.Pivot.Opacity = 0.6;
            uiHelper.ShowProgressIndicator("Logging in......", this);
            string userName = userNameText.Text;
            string password = passwordText.Password;
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    serverHelper = new ServerHelper();
                    Token token = await serverHelper.GetToken(userName, password);
                    AppSettings settings = new AppSettings();
                    if (token != null)
                    {
                        if (token.Error == null)
                        {
                            settings.AddOrUpdateValue("token", token.AccessToken);
                            settings.AddOrUpdateValue("issued", token.Issued);
                            settings.AddOrUpdateValue("expires", token.Expires);
                            settings.AddOrUpdateValue("isLogin", true);
                            settings.Save();
                            serverHelper = new ServerHelper();
                            uiHelper.ShowProgressIndicator("Getting user information......", this);
                            UserInfo userInfo = await serverHelper.GetUserInfo();
                            userInfo.ProfilePic = await serverHelper.GetProfilePic(userInfo.ProfilePicUrl);
                            using (DataBaseContext db = new DataBaseContext(DataBaseContext.DBConnectionString))
                            {
                                if (!db.DatabaseExists())
                                {
                                    //Create the database
                                    db.CreateDatabase();
                                }
                                db.UserInfo.InsertOnSubmit(userInfo);
                                db.SubmitChanges();
                            }
                            uiHelper.ShowProgressIndicator("Getting friends list......", this);
                            appHelper = new AppHelper();
                            await appHelper.UpdateFriends();
                            await ConnectionHelper.Connect();
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        }
                        else
                        {
                            settings.AddOrUpdateValue("isLogin", false);
                            settings.Save();
                            Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show(token.Error);
                            });
                        }
                    }
                    else
                    {
                        settings.AddOrUpdateValue("isLogin", false);
                        settings.Save();
                        Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("Sorry, unable to contact the server.");
                        });
                    }
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Sorry, no network found. Please check your connection settings.");
                    });
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
            this.Pivot.IsHitTestVisible = true;
            this.Pivot.Opacity = 1;
            uiHelper.HideProgressIndicator(this);
        }

        private async void registerButton_Click(object sender, RoutedEventArgs e)
        {
            this.Pivot.IsHitTestVisible = false;
            this.Pivot.Opacity = 0.6;
            uiHelper.ShowProgressIndicator("Registering......", this);
            serverHelper = new ServerHelper();
            await serverHelper.Register(
                registerCountryCode.Text, registerPhoneNumber.Text, registerName.Text, registerEmail.Text, registerPassword.Password, registerConfirmPassword.Password);
            uiHelper.HideProgressIndicator(this);
            this.Pivot.IsHitTestVisible = true;
            this.Pivot.Opacity = 1;
            this.Pivot.SelectedIndex = 0;
        }
    }
}