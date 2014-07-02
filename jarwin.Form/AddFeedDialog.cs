using jarwin.DAL;
using jarwin.ObjectFactory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace jarwin
{
    public partial class AddFeedDialog : System.Windows.Forms.Form
    {
        public JarwinDataContext dataContext
        {
            get;
            private set;
        }

        public AddFeedDialog(JarwinDataContext dataContextMain)
        {
            InitializeComponent();
            dataContext = dataContextMain;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                infoLabel.Text = "Processing file...";
                CreateFeedFromRss(textBox1.Text);
                infoLabel.Text = "File processed successfully!";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            var filepath = fileDialog.ShowDialog();
            textBox1.Text = fileDialog.FileName;
        }

        private void CreateFeedFromRss(string inputUri)
        {
            // Using the Rss feed xml, create an Rss object.
            Rss rss = new Rss(inputUri);
            dataContext.Feed.InsertOnSubmit(rss.feed);
            dataContext.SubmitChanges();

            int feedID = rss.feed.feedID; // Value provided by SQL Server identity column.
            int feedItemID = 0;

            foreach (var feedItem in rss.feedItems)
            {
                feedItem.feedID = feedID;
                feedItem.feedItemID = feedItemID;
                dataContext.FeedItem.InsertOnSubmit(feedItem);

                feedItemID += 1;
            }

            dataContext.SubmitChanges();
        }
    }
}
