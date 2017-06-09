using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSTracker.Model
{
    class DataBaseContext : DataContext
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/Database.sdf";

        // Pass the connection string to the base class.
        public DataBaseContext(string connectionString)
            : base(connectionString)
        { }

        public Table<UserInfo> UserInfo;
        public Table<Friends> Friends;
        public Table<BlockList> BlockList;
    }
}
