using jarwin.DAL;
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

        private void AddFeedDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            XDocument feedXML = null;

            // Try and load the provided feed.
            try
            {
                feedXML = XDocument.Load(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (feedXML != null)
            {

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
            // Using the Rss feed xml, create a Feed object.

        }
    }
}
