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
using jarwin.State;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Threading;

namespace jarwin.Form
{
    public partial class jarwin : System.Windows.Forms.Form, IDisposable
    {
        public JarwinDataContext dataContext { get; private set; }
        public LoggingConfiguration loggingConfiguration { get; set; }
        public LogWriter logWriter { get; set; }
        private TreeNode currentNode { get; set; }
        private StateAbstract currentState { get; set; }
        private delegate void SetTextCallback(string text);
        private Thread updateThread = null;

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
            this.toolStripStatusLabel2.Text = String.Empty;
            currentState = new StateNormal(false);
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
                node.ToolTipText = feed.description;
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
            else if (e.Node.Name.ToUpper() != "MY FEEDS")
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    clearDataGridView();
                    clearBrowserView();
                }

                treeView1.SelectedNode = e.Node;

                // Load feed items into the data grid view for the selected node:

                Utility.Utility utility = new Utility.Utility();

                try
                {
                    using (JarwinDataContext cxt = new JarwinDataContext(utility.GetAppSetting("connectionString2")))
                    {
                        var feedItems =
                            from feedItem in cxt.FeedItem
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
                            // Set the WebBrowser view to contain the first feed entry.
                            FeedItem item = (FeedItem)dataGridView1.Rows[0].Tag;
                            webBrowser.DocumentText = String.IsNullOrEmpty(item.content) ? item.description : item.content;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sorry, can't display feed items at this time.", "jarwin");

                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Failed to display feed items.  Exception type = {0}.  Exception msg = {1}.", ex.GetType(), ex.Message));
                    }
                }
            }
            else
            {
                // Do nothing.
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

            int feedID = 0;
            bool rootNode = false;

            try
            {
                feedID = (int)currentNode.Tag;
            }
            catch (NullReferenceException ex) // Root node doesn't have an identifier - "My Feeds".
            {
                MessageBox.Show("Sorry, cannot delete this feed.", "jarwin");

                if (logWriter.IsLoggingEnabled())
                {
                    logWriter.Write(String.Format("ERROR :: Cannot delete Feed - it's the root node.  Exception type = {0}.  Exception msg = {1}.  feedID = {2}", ex.GetType(), ex.Message, feedID));
                }

                rootNode = true;
            }

            if (!rootNode)
            {
                DialogResult result = MessageBox.Show("Are you sure you want delete this feed?", "jarwin", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    Rss rss = new Rss();

                    try
                    {
                        rss.Delete(feedID, dataContext);
                        refreshTreeView();
                        clearDataGridView();
                        clearBrowserView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Sorry, failed to delete the feed.", "jarwin");

                        if (logWriter.IsLoggingEnabled())
                        {
                            logWriter.Write(String.Format("ERROR :: Failed to delete feed.  Exception type = {0}.  Exception msg = {1}.  feedID = {2}", ex.GetType(), ex.Message, feedID));
                        }
                    }
                }
            }
        }

        private async void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // "Sync all" button pressed.  Update Feed and associated FeedItems.

            int failedSyncCount = 0;
            int feedsCount = 0;
            Utility.Utility utility = new Utility.Utility();
            JarwinDataContext dataContextLocal = new JarwinDataContext(utility.GetAppSetting("connectionString2"));

            // Set status to "syncing".
            currentState = new StateSyncing(currentState.isRefreshRequired);
            this.updateThread = new Thread(new ThreadStart(this.threadProcSafe));
            this.updateThread.Start();

            var feeds =
                from feed in dataContextLocal.Feed
                where feed.status.ToUpper() == "ACTIVE"
                && feed.lastDownloadDateTime.AddHours((double)feed.updateFrequency) <= DateTime.Now
                select feed;

            feedsCount = feeds.Count();

            Task[] tasks = new Task[feeds.Count()];
            int index = -1;

            foreach (var feed in feeds)
            {
                // Run each update on a separate thread, to keep the UI responsive.
                // And to run multiple updates on different threads to increase performance.

                tasks[++index] = Task.Run(async () =>
                {
                    try
                    {
                        Rss rss = new Rss();
                        await rss.Update(feed.feedID, new JarwinDataContext(utility.GetAppSetting("connectionString2")));
                    }
                    catch (Exception ex)
                    {
                        // Update state of this feed to "FAILED_SYNCING"??

                        ++failedSyncCount;

                        if (logWriter.IsLoggingEnabled())
                        {
                            logWriter.Write(String.Format("ERROR :: Failed to update feed.  Exception type = {0}.  Exception msg = {1}.  feedID = {2}", ex.GetType(), ex.Message, feed.feedID));
                        }
                    }
                });
            }

            await Task.WhenAll(tasks).ContinueWith( (t) =>
            {
                if (feedsCount == 0)
                {
                    // Nothing marked for update.
                    currentState = new StateNothingToSync(currentState.isRefreshRequired);
                    this.updateThread = new Thread(new ThreadStart(this.threadProcSafe));
                    this.updateThread.Start();
                }
                else if (failedSyncCount < feedsCount)
                {
                    currentState = new StateRefreshRequired();
                    this.updateThread = new Thread(new ThreadStart(this.threadProcSafe));
                    this.updateThread.Start();
                }
                else
                {
                    currentState = new StateFailedSyncing(currentState.isRefreshRequired);
                    this.updateThread = new Thread(new ThreadStart(this.threadProcSafe));
                    this.updateThread.Start();
                }
            }
            );

            if (feedsCount == 0)
            {
                // Nothing marked for update.
                currentState = new StateNothingToSync(currentState.isRefreshRequired);
                this.updateThread = new Thread(new ThreadStart(this.threadProcSafe));
                this.updateThread.Start();
            }
        }

        private void threadProcSafe()
        {
            // Ensure that the UI thread is used to update the status bar.
            this.updateStatusText(currentState.description);
        }

        private void updateStatusText(string text)
        {
            if (this.statusStrip.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(updateStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.toolStripStatusLabel2.Text = currentState.description;
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void feedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentState.isRefreshRequired)
            {
                refreshAllToolStripMenuItem.Enabled = true;
            }
            else
            {
                refreshAllToolStripMenuItem.Enabled = false;
            }
        }

        private void refreshAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Refresh all button selected.
            refreshTreeView();
            clearDataGridView();
            clearBrowserView();

            currentState = new StateNormal(false);
            this.updateThread = new Thread(new ThreadStart(this.threadProcSafe));
            this.updateThread.Start();
        }
    }
}
