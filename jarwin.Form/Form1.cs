using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using jarwin.DAL;
using jarwin.Form;
using jarwin.ObjectFactory;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace jarwin.Form
{
    public partial class jarwin : System.Windows.Forms.Form, IDisposable
    {
        public JarwinDataContext dataContext
        {
            get;
            private set;
        }

        public LoggingConfiguration loggingConfiguration { get; set; }
        public LogWriter logWriter { get; set; }

        private TreeNode currentNode
        { 
            get;
            set;
        }

        private void Dispose()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }

        public jarwin()
        {
            InitializeComponent();
            Utility.Utility utility = new Utility.Utility();
            dataContext = new JarwinDataContext(utility.GetAppSetting("connectionString2"));
            loggingConfiguration = Utility.Utility.BuildProgrammaticConfig();
            logWriter = new LogWriter(loggingConfiguration);
        }

        private void jarwin_Load(object sender, EventArgs e)
        {
            // Form load event.
            initTreeView();
        }

        private void initTreeView()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            var feedData =
                from feed in dataContext.Feed
                where feed.status.ToUpper() != "INACTIVE"
                select feed;

            foreach (var feed in feedData)
            {
                TreeNode node = new TreeNode(feed.title);
                node.Tag = feed.feedID;
                nodes.Add(node);
            }

            TreeNode treeNode = new TreeNode("My Feeds", nodes.ToArray<TreeNode>());
            treeView1.Nodes.Add(treeNode);
        }

        private void clearDataGridView()
        {
            dataGridView1.Rows.Clear();
        }

        private void clearBrowserView()
        {
            webBrowser.DocumentText = String.Empty;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node.Name.ToUpper() != "MY FEEDS")
            {
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                currentNode = treeView1.GetNodeAt(e.Location);
            }
            else
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    clearDataGridView();
                    clearBrowserView();
                }

                treeView1.SelectedNode = e.Node;

                // Load feed items into the data grid view for the selected node:

                var feedItems =
                    from feedItem in dataContext.FeedItem
                    where feedItem.feedID == (int)e.Node.Tag
                    select feedItem;

                int index = 0;

                try
                {
                    if (feedItems.Count() > 0)
                    {
                        foreach (var item in feedItems)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells[0].Value = item.publishedDateTime;
                            dataGridView1.Rows[index].Cells[1].Value = item.title;

                            dataGridView1.Rows[index].Tag = item;

                            ++index;
                        }
                    }
                }
                catch (NullReferenceException)
                { }

                if (dataGridView1.Rows.Count > 0)
                {
                    // Set the WebBrowser view to contain the first blog entry.
                    FeedItem item = (FeedItem)dataGridView1.Rows[0].Tag;
                    webBrowser.DocumentText = String.IsNullOrEmpty(item.content) ? item.description : item.content;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // When the user clicks on a feed in the data grid,
            // populate the browser view with details of the feed.
            FeedItem item = (FeedItem)dataGridView1.Rows[e.RowIndex].Tag;
            webBrowser.DocumentText = String.IsNullOrEmpty(item.content) ? item.description : item.content;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // File -> Exit.
            Application.Exit();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Feed -> Add.
            AddFeedDialog addFeedDialog = new AddFeedDialog(dataContext);
            addFeedDialog.Show();
            addFeedDialog.FormClosing += new FormClosingEventHandler(refreshTreeView);
        }

        private void refreshTreeView(object sender, FormClosingEventArgs e) // Event handler for addFeedDialog form closing event.
        {
            // Reload the tree view to include any additonal Feeds.
            refreshTreeView();
        }

        private void refreshTreeView()
        {
            // Reload the tree view to include any additonal Feeds.

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes.RemoveAt(i);
            }

            initTreeView();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Delete feed selected from tree view context menu.

            int feedID = (int)currentNode.Tag;

            DialogResult result = MessageBox.Show("Are you sure you want delete this feed?", "jarwin", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                Rss rss = new Rss();
                
                try
                {
                    rss.Delete(feedID, dataContext);
                    refreshTreeView();
                    clearDataGridView();
                }
                catch
                {
                    MessageBox.Show("Sorry, failed to delete the feed.", "jarwin");
                    // TODO: implement logging.
                }
            }
        }

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Sync all" button pressed.  Update Feed and associated FeedItems.

            // TODO: set app state to "SYNCING".
            // This event should trigger message bar to display suitable text.
            
            //Rss rss = new Rss();

            var feeds =
                from feed in dataContext.Feed
                where feed.status.ToUpper() == "ACTIVE"
                && feed.lastDownloadDateTime.AddHours((double)feed.updateFrequency) <= DateTime.Now
                select feed;

            Task[] tasks = new Task[feeds.Count()];
            int index = -1;
            Utility.Utility utility = new Utility.Utility();

            foreach (var feed in feeds)
            {
                // Run each update on a separate thread, to keep the UI responsive.
                // And to run multiple updates on different threads to increase performance.

                tasks[++index] = Task.Run(() =>
                {
                    try
                    {
                        // Don't want to await this method call.
                        Rss rss = new Rss();
                        Task<bool> result = rss.Update(feed.feedID, new JarwinDataContext(utility.GetAppSetting("connectionString2")));
                    }
                    catch (Exception)
                    {
                        // Update state of this feed to "FAILED_SYNCING"??
                        logWriter.Write(String.Format("Error :: Failed to update Rss with feedID = {0}", feed.feedID));
                    }
                });
            }

            Task.WaitAll(tasks);

            // TODO: set app state to "NOT_SYNCING".
            // This event should trigger message bar to display suitable text.

            // TODO: prompt before refreshing the tree view.
            // TODO: if user selects to not update the tree view, update status to "TREE_REFRESH_PENDING" and update message bar status message.
            refreshTreeView();
            //clearDataGridView();
            //clearBrowserView();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
