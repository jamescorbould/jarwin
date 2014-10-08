using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SQLite;

namespace jarwin.DAL
{
    [Database]
    public partial class JarwinDataContext : DataContext
    {
        public JarwinDataContext(string connection)
            : base(connection)
        {
            var sqliteConnection = new SQLiteConnection(connection);
            new DataContext(sqliteConnection);
        }

        public Table<Feed> Feed;
        public Table<FeedItem> FeedItem;
        public Table<FeedHistory> FeedHistory;
        public Table<FeedItemHistory> FeedItemHistory;
    }
}
