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

namespace jarwin.Form
{
    public partial class jarwin : System.Windows.Forms.Form
    {
        public JarwinDataContext dataContext
        {
            get;
            private set;
        }

        public jarwin()
        {
            InitializeComponent();
            Utility.Utility utility = new Utility.Utility();
            dataContext = new JarwinDataContext(utility.GetAppSetting("connectionString"));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void jarwin_Load(object sender, EventArgs e)
        {
            // Form load event.
            initTreeView();
        }

        private void initTreeView()
        {
            TreeNode treeNode = new TreeNode("My Feeds");
            treeView1.Nodes.Add(treeNode);

            TreeNode node2 = new TreeNode("C#");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode[] array = new TreeNode[] { node2, node3 };

            treeNode = new TreeNode("Dot Net Perls", array);
            treeView1.Nodes.Add(treeNode);

            var feedQuery =
                from feed in dataContext.Feed
                where feed
                select cust.CompanyName;

            foreach (var customer in companyNameQuery)
            {
                Console.WriteLine(customer);
            }
        }

    }
}
