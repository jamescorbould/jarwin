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
        public AddFeedDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                infoLabel.Text = "Processing file...";

                try
                {
                    CreateFeedFromRss(textBox1.Text);
                    infoLabel.Text = "File processed successfully!";
                }
                catch
                {
                    infoLabel.Text = "File processing failed!  Sorry, can't process that file.";
                    // TODO: do some logging here.
                }
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

            using (mainEntities context = new mainEntities())
            {
                context.feeds.Add(rss.feed);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error processing feed.\n\nError message as follows: {0}", ex.Message), "jarwin - Error Processing Feed");
                    throw;
                }

                int feedID = (int)rss.feed.feed_id; // Value provided by identity column.
                int feedItemID = 0;

                foreach (var feedItem in rss.feedItems)
                {
                    feedItem.feed_id = feedID;
                    feedItem.feed_item_id = feedItemID;
                    context.feed_item.Add(feedItem);

                    feedItemID += 1;
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error process feed.\n\nError message as follows: {0}", ex.Message), "jarwin - Error Processing Feed Items");
                    throw;
                }
            }
        }
    }
}
