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

namespace jarwin.Form
{
    public partial class jarwin : System.Windows.Forms.Form
    {
        public JarwinDataContext dataContext
        {
            get;
            private set;
        }

        private TreeNode currentNode
        { 
            get;
            set;
        }

        public jarwin()
        {
            InitializeComponent();
            Utility.Utility utility = new Utility.Utility();
            dataContext = new JarwinDataContext(utility.GetAppSetting("connectionString2"));
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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node.Name != "my feeds")
            {
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                currentNode = treeView1.GetNodeAt(e.Location);
            }
            else
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    clearDataGridView();
                }

                treeView1.SelectedNode = e.Node;

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
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FeedItem item = (FeedItem)dataGridView1.Rows[e.RowIndex].Tag;
            webBrowser.Text = String.IsNullOrEmpty(item.content) ? item.description : item.content;
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

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes.RemoveAt(i);
            }

            initTreeView();
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
            int feedID = (int)currentNode.Tag;

            DialogResult result = MessageBox.Show("Are you sure you want delete this feed?", "jarwin", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                Rss rss = new Rss();
                
                if (!rss.Delete(feedID, dataContext))
                {
                    MessageBox.Show("Sorry, failed to delete the feed.", "jarwin");
                }
                else
                {
                    refreshTreeView();
                    clearDataGridView();
                }
            }
        }

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
