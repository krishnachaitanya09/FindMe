using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace GPSTracker.Helpers
{
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;
        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            // URI association launch for my app detected
            if (tempUri.Contains("findme:Location?phonenumber="))
            {
                int PhoneNumberIndex = tempUri.IndexOf("phonenumber=") + 12;
                string PhoneNumber = tempUri.Substring(PhoneNumberIndex);
                int NameIndex = tempUri.IndexOf("Name=") + 5;
                return new Uri("/LocationPage.xaml?phonenumber=" + PhoneNumber, UriKind.Relative);
            }
            // Otherwise perform normal launch.
            return uri;
        }
    }
}
