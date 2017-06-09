using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GPSTracker.Model;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;
using System.Net.Http.Headers;
using System.Windows.Media.Imaging;

namespace GPSTracker.Helpers
{
    class ServerHelper
    {
        private HttpClient httpClient;
        private AppSettings settings;

        public ServerHelper()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://techcryptic.com/findme");
            settings = new AppSettings();
            httpClient.DefaultRequestHeaders.Authorization =
               new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.GetValue<string>("token"));
        }

        public async Task<HttpResponseMessage> Post(Uri uri, HttpContent httpContent)
        {
            return await httpClient.PostAsync(uri, httpContent);
        }

        public async Task<HttpResponseMessage> Post(Uri uri, MultipartFormDataContent httpContent)
        {
            return await httpClient.PostAsync(uri, httpContent);
        }

        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            return await httpClient.GetAsync(uri);
        }

        public async Task<Token> GetToken(string userName, string password)
        {
            HttpContent httpContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });
            HttpResponseMessage message = await Post(new Uri("/Token", UriKind.Relative), httpContent);
            //message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
            Token token = JsonConvert.DeserializeObject<Token>(result);
            return token;
        }

        public async Task Register(string countryCode, string phoneNumber, string name, string email, string password, string confirmPassword)
        {
            HttpContent httpContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("CountryCode", countryCode),
                    new KeyValuePair<string, string>("PhoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("Name", name),
                    new KeyValuePair<string, string>("Email", email),
                    new KeyValuePair<string, string>("Password", password),
                    new KeyValuePair<string, string>("ConfirmPassword", confirmPassword),
                    new KeyValuePair<string, string>("grant_type", "password")
                });
            HttpResponseMessage message = await Post(new Uri("/api/Account/Register", UriKind.Relative), httpContent);
            //message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
        }


        public async Task<UserInfo> GetUserInfo()
        {
            HttpResponseMessage message = await Get(new Uri("/api/Account/UserInfo", UriKind.Relative));
            //message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(result);
            return userInfo;
        }

        public async Task UploadProfilePic(Stream photo)
        {
            MultipartFormDataContent content =
            new MultipartFormDataContent();
            photo.Position = 0;
            content.Add(new StreamContent(photo), "media", "profilepic.jpg");
            HttpResponseMessage message = await Post(new Uri("/api/Account/ChangeProfilePic", UriKind.Relative), content);
            message.EnsureSuccessStatusCode();
        }

        public async Task<byte[]> GetProfilePic(string profilePicUrl)
        {
            HttpResponseMessage message = await Get(new Uri(profilePicUrl, UriKind.Relative));
            message.EnsureSuccessStatusCode();
            byte[] stream = await message.Content.ReadAsByteArrayAsync();
            return stream;
        }

        public async Task<List<Friends>> GetFriends(List<Friends> friendslList)
        {
            HttpResponseMessage message = await httpClient.PostAsJsonAsync(new Uri("/api/Friends/PostFriends", UriKind.Relative), friendslList);
            message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
            List<Friends> friends = JsonConvert.DeserializeObject<List<Friends>>(result);
            return friends;
        }

        public async Task<Location> GetFriendsLocation(string phoneNumber)
        {
            HttpResponseMessage message = await Get(new Uri("/api/Friends/GetRecentLocation?phoneNumber=" + Uri.EscapeDataString(phoneNumber), UriKind.Relative));
            message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
            Location location = JsonConvert.DeserializeObject<Location>(result);
            return location;

        }

        public async Task SendBlockList(List<BlockList> blockList)
        {
            HttpResponseMessage message = await httpClient.PostAsJsonAsync(new Uri("/api/Friends/BlockFriends", UriKind.Relative), blockList);
            message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
        }

        public async Task RemoveFromBlockList(List<BlockList> blockList)
        {
            HttpResponseMessage message = await httpClient.PostAsJsonAsync(new Uri("/api/Friends/RemoveFromBlockList", UriKind.Relative), blockList);
            message.EnsureSuccessStatusCode();
            string result = await message.Content.ReadAsStringAsync();
        }

        public async Task Logout()
        {
            HttpResponseMessage message = await Post(new Uri("/api/Account/Logout", UriKind.Relative), null);
            message.EnsureSuccessStatusCode();
        }
    }
}
